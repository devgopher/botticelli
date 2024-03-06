using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Botticelli.AI.AIProvider;
using Botticelli.AI.Message;
using Botticelli.AI.YaGpt.Message.YaGpt;
using Botticelli.AI.YaGpt.Settings;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Responses;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Botticelli.AI.YaGpt.Provider;

public class YaGptProvider : GenericAiProvider<YaGptSettings>
{
    private const string SystemRole = "system";
    private const string UserRole = "user";
    private const string Completion = "completion";

    public YaGptProvider(IOptionsSnapshot<YaGptSettings> gptSettings,
        IHttpClientFactory factory,
        ILogger<YaGptProvider> logger,
        IBusClient bus) : base(gptSettings,
        factory,
        logger,
        bus)
    {
    }

    public override string AiName => "yagpt";

    public override async Task SendAsync(AiMessage message, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(message.Body?.Trim()))
        {
            Logger.LogError($"{nameof(SendAsync)}() body is null or empty!");

            await Bus.SendResponse(new SendMessageResponse(message.Uid)
                {
                    IsPartial = Settings.Value.ExpectPartialResponses,
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

            client.BaseAddress = new Uri(Settings.Value.Url);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Settings.Value.ApiKey);

            var yaGptMessage = new YaGptInputMessage
            {
                ModelUri = Settings.Value.Model,
                Messages = new List<YaGptMessage>
                {
                    new()
                    {
                        Role = SystemRole,
                        Text = Settings.Value.Instruction
                    },
                    new()
                    {
                        Role = UserRole,
                        Text = message.Body
                    }
                },
                CompletionOptions = new CompletionOptions()
                {
                    MaxTokens = Settings.Value.MaxTokens,
                    Stream = Settings.Value.StreamGeneration,
                    Temperature = Settings.Value.Temperature
                }
            };

            yaGptMessage.Messages.AddRange(message.AdditionalMessages?.Select(m => new YaGptMessage()
            {
                Role = UserRole,
                Text = m.Body
            }) ?? new List<YaGptMessage>());

            var content = JsonContent.Create(yaGptMessage);

            Logger.LogDebug($"{nameof(SendAsync)}({message.ChatIds}) content: {content.Value}");

            var response = await client.PostAsync(Url.Combine($"{Settings.Value.Url}", Completion),
                content,
                token);

            if (response.IsSuccessStatusCode)
            {
                var text = new StringBuilder();

                var outStream = await response.Content.ReadAsStreamAsync(token);
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

                    if (part?.Result?.Alternatives?.Any(c => c.Message.Text.Contains("ALTERNATIVE_STATUS_FINAL")) ==
                        true)
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