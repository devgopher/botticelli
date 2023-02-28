using System.Net.Http.Json;
using Botticelli.AI.Exceptions;
using Botticelli.AI.Message;
using Botticelli.AI.Message.GptJ;
using Botticelli.AI.Settings;
using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Responses;
using Flurl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.AIProvider;

public class GptJProvider : GenericAiProvider
{
    private readonly Random _temperatureRandom = new(DateTime.Now.Millisecond);
    public GptJProvider(IOptionsMonitor<AiSettings> settings,
                        IHttpClientFactory factory,
                        ILogger<GptJProvider> logger,
                        IBotticelliBusClient bus) : base(settings,
                                                         factory,
                                                         logger,
                                                         bus)
    {
    }

    public override string AiName => "gptj";

    public override async Task SendAsync(AiMessage message, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(message.Body)) throw new AiException($"{nameof(SendAsync)}() body is null or empty!");

        try
        {
            _logger.LogDebug($"{nameof(SendAsync)}({message.ChatId}) started");

            using var client = _factory.CreateClient();

            client.BaseAddress = new Uri(_settings.CurrentValue.Url);

            var content = JsonContent.Create(new GptJInputMessage
            {
                Text = message.Body,
                GenerateTokensLimit = 50 + (int) (message.Body.Length * 5),
                TopP = 0.5,
                TopK = 0,
                Temperature = (_temperatureRandom.Next(0, 900) + 100) / 1000.0
            });

            _logger.LogDebug($"{nameof(SendAsync)}({message.ChatId}) content: {content.Value}");

            var response = await client.PostAsync(Url.Combine($"{_settings.CurrentValue.Url}", "generate"),
                                                  content,
                                                  token);

            if (response.IsSuccessStatusCode)
            {
                var outMessage = await response.Content.ReadFromJsonAsync<GptJOutputMessage>();

                await _bus.SendResponse(new SendMessageResponse(message.Uid)
                                        {
                                            Message = new Shared.ValueObjects.Message(message.Uid)
                                            {
                                                ChatId = message.ChatId,
                                                Subject = message.Subject,
                                                Body = outMessage?.Completion,
                                                Attachments = null,
                                                From = null,
                                                ForwardFrom = null
                                            }
                                        },
                                        token);
            }
            else
                await _bus.SendResponse(new SendMessageResponse(Guid.NewGuid().ToString())
                                        {
                                            Message = new Shared.ValueObjects.Message(Guid.NewGuid().ToString())
                                            {
                                                ChatId = message.ChatId,
                                                Subject = message.Subject,
                                                Body = "Error getting a response from Gpt-J!",
                                                Attachments = null,
                                                From = null,
                                                ForwardFrom = null
                                            }
                                        },
                                        token);

            _logger.LogDebug($"{nameof(SendAsync)}({message.ChatId}) finished");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}