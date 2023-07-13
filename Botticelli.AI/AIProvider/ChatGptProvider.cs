using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Botticelli.AI.Message;
using Botticelli.AI.Message.ChatGpt;
using Botticelli.AI.Settings;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Responses;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.AIProvider;

public class ChatGptProvider : GenericAiProvider
{
    private readonly IOptionsMonitor<ChatGptSettings> _gptSettings;
    private readonly Random _temperatureRandom = new(DateTime.Now.Millisecond);

    public ChatGptProvider(IOptionsMonitor<ChatGptSettings> gptSettings,
                           IHttpClientFactory factory,
                           ILogger<ChatGptProvider> logger,
                           IBotticelliBusClient bus) : base(gptSettings,
                                                            factory,
                                                            logger,
                                                            bus)
        => _gptSettings = gptSettings;

    public override string AiName => "chatgpt";

    public override async Task SendAsync(AiMessage message, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(message.Body?.Trim()))
        {
            _logger.LogError($"{nameof(SendAsync)}() body is null or empty!");

            await _bus.SendResponse(new SendMessageResponse(message.Uid)
                                    {
                                        Message = new Shared.ValueObjects.Message(message.Uid)
                                        {
                                            ChatId = message.ChatId,
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
            _logger.LogDebug($"{nameof(SendAsync)}({message.ChatId}) started");

            using var client = _factory.CreateClient();

            client.BaseAddress = new Uri(_settings.CurrentValue.Url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _gptSettings.CurrentValue.ApiKey);

            var content = JsonContent.Create(new ChatGptInputMessage
            {
                Model = _gptSettings.CurrentValue.Model,
                Messages = new List<ChatGptMessage>
                {
                    new()
                    {
                        Role = "user",
                        Content = message.Body
                    }
                },
                Temperature = _gptSettings?.CurrentValue?.Temperature ?? (_temperatureRandom.Next(0, 900) + 100) / 1000.0
            });

            _logger.LogDebug($"{nameof(SendAsync)}({message.ChatId}) content: {content.Value}");

            var response = await client.PostAsync(Url.Combine($"{_settings.CurrentValue.Url}", "completions"),
                                                  content,
                                                  token);

            if (response.IsSuccessStatusCode)
            {
                var outMessage = await response.Content.ReadFromJsonAsync<ChatGptOutputMessage>();

                var text = new StringBuilder();
                text.AppendJoin(' ',
                                outMessage?
                                        .Choices?
                                        .Select(c => c.Message.Content));

                await _bus.SendResponse(new SendMessageResponse(message.Uid)
                                        {
                                            Message = new Shared.ValueObjects.Message(message.Uid)
                                            {
                                                ChatId = message.ChatId,
                                                Subject = message.Subject,
                                                Body = text.ToString(),
                                                Attachments = null,
                                                From = null,
                                                ForwardedFrom = null,
                                                ReplyToMessageUid = message.ReplyToMessageUid
                                            }
                                        },
                                        token);
            }
            else
            {
                var reason = await response.Content.ReadAsStringAsync();

                await _bus.SendResponse(new SendMessageResponse(message.Uid)
                                        {
                                            Message = new Shared.ValueObjects.Message(message.Uid)
                                            {
                                                ChatId = message.ChatId,
                                                Subject = message.Subject,
                                                Body = "Error getting a response from ChatGpt!",
                                                Attachments = null,
                                                From = null,
                                                ForwardedFrom = null,
                                                ReplyToMessageUid = message.ReplyToMessageUid
                                            }
                                        },
                                        token);
            }

            _logger.LogDebug($"{nameof(SendAsync)}({message.ChatId}) finished");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await _bus.SendResponse(new SendMessageResponse(message.Uid)
                                    {
                                        Message = new Shared.ValueObjects.Message(message.Uid)
                                        {
                                            ChatId = message.ChatId,
                                            Subject = message.Subject,
                                            Body = "Error getting a response from ChatGpt!",
                                            Attachments = null,
                                            From = null,
                                            ForwardedFrom = null
                                        }
                                    },
                                    token);
        }
    }
}