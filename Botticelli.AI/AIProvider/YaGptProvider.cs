using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Botticelli.AI.Message;
using Botticelli.AI.Message.ChatGpt;
using Botticelli.AI.Message.YaGpt;
using Botticelli.AI.Settings;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Botticelli.AI.AIProvider;

public class YaGptProvider : GenericAiProvider
{
    private readonly IOptionsMonitor<YaGptSettings> _gptSettings;
    private readonly Random _temperatureRandom = new(DateTime.Now.Millisecond);
    private const string SystemRole = "system";
    private const string UserRole = "user";
    private const string Completion = "completion";

    public YaGptProvider(IOptionsMonitor<YaGptSettings> gptSettings,
        IHttpClientFactory factory,
        ILogger<ChatGptProvider> logger,
        IBusClient bus) : base(gptSettings,
        factory,
        logger,
        bus)
    {
        _gptSettings = gptSettings;
    }

    public override string AiName => "yagpt";

    public override async Task SendAsync(AiMessage message, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(message.Body?.Trim()))
        {
            Logger.LogError($"{nameof(SendAsync)}() body is null or empty!");

            await Bus.SendResponse(new SendMessageResponse(message.Uid)
                {
                    IsPartial = _gptSettings.CurrentValue.ExpectPartialResponses,
                    Message = new Shared.ValueObjects.Message(message.Uid)
                    {
                        ChatIds = message.ChatIds,
                        Subject = message.Subject,
                        Body = "Body is null or empty!",
                        Attachments = null,
                        From = null,
                        ForwardedFrom = null,
                        ReplyToMessageUid = message.ReplyToMessageUid
                    }
                },
                token);

            return;
        }

        try
        {
            Logger.LogDebug($"{nameof(SendAsync)}({message.ChatIds}) started");

            using var client = Factory.CreateClient();

            client.BaseAddress = new Uri(Settings.CurrentValue.Url);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _gptSettings.CurrentValue.ApiKey);

            var yaGptMessage = new YaGptInputMessage
            {
                ModelUri = _gptSettings.CurrentValue.Model,
                Messages = new List<YaGptMessage>
                {
                    new()
                    {
                        Role = SystemRole,
                        Text = _gptSettings.CurrentValue.Instruction
                    },
                    new()
                    {
                        Role = UserRole,
                        Text = message.Body
                    }
                },
                CompletionOptions = new CompletionOptions()
                {
                    MaxTokens = _gptSettings.CurrentValue.MaxTokens,
                    Stream = _gptSettings.CurrentValue.StreamGeneration,
                    Temperature = _gptSettings?.CurrentValue?.Temperature ??
                                  (_temperatureRandom.Next(0, 900) + 100) / 1000.0
                }
            };

            yaGptMessage.Messages.AddRange(message.AdditionalMessages?.Select(m => new  YaGptMessage()
            {
                Role = UserRole,
                Text = m.Body
            }) ?? new List<YaGptMessage>());

            var content = JsonContent.Create(yaGptMessage);
            
            Logger.LogDebug($"{nameof(SendAsync)}({message.ChatIds}) content: {content.Value}");

            var response = await client.PostAsync(Url.Combine($"{Settings.CurrentValue.Url}", Completion),
                content,
                token);

            if (response.IsSuccessStatusCode)
            {
                var text = new StringBuilder();
                
                var outStream = await response.Content.ReadAsStreamAsync(cancellationToken: token);
                var serializer = new JsonSerializer();
                using var sr = new StreamReader(outStream);
                await using var reader = new JsonTextReader(sr)
                {
                    SupportMultipleContent = true
                };
                
                while (await reader.ReadAsync(token))
                {
                    if (reader.TokenType != JsonToken.StartObject) 
                        continue;
                    var part = serializer.Deserialize<YaGptOutputMessage>(reader);
                    
                    text.AppendJoin(' ',
                        part?.Result?
                            .Alternatives?
                            .Select(c => c.Message.Text) ?? Array.Empty<string>());

                    await Bus.SendResponse(new SendMessageResponse(message.Uid)
                        {
                            Message = new Shared.ValueObjects.Message(message.Uid)
                            {
                                ChatIds = message.ChatIds,
                                Subject = message.Subject,
                                Body = text.ToString(),
                                Attachments = null,
                                From = null,
                                ForwardedFrom = null,
                                ReplyToMessageUid = message.ReplyToMessageUid
                            }
                        },
                        token);

                    if (part?.Result?.Alternatives?.Any(c => c.Message.Text.Contains("ALTERNATIVE_STATUS_FINAL")) == true)
                        break;
                }
            }
            else
            {
                var reason = await response.Content.ReadAsStringAsync(token);

                await Bus.SendResponse(new SendMessageResponse(message.Uid)
                    {
                        Message = new Shared.ValueObjects.Message(message.Uid)
                        {
                            ChatIds = message.ChatIds,
                            Subject = message.Subject,
                            Body = $"Error getting a response from YaGpt: {reason}!",
                            Attachments = null,
                            From = null,
                            ForwardedFrom = null,
                            ReplyToMessageUid = message.ReplyToMessageUid
                        }
                    },
                    token);
            }

            Logger.LogDebug($"{nameof(SendAsync)}({message.ChatIds}) finished");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            await Bus.SendResponse(new SendMessageResponse(message.Uid)
                {
                    Message = new Shared.ValueObjects.Message(message.Uid)
                    {
                        ChatIds = message.ChatIds,
                        Subject = message.Subject,
                        Body = "Error getting a response from YaGpt!",
                        Attachments = null,
                        From = null,
                        ForwardedFrom = null
                    }
                },
                token);
        }
    }
}