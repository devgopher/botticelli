using Botticelli.AI.Exceptions;
using Botticelli.AI.Message;
using Botticelli.AI.Message.GptJ;
using Botticelli.AI.Settings;
using Botticelli.Bot.Interfaces.Agent;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.AI.AIProvider;

public class GptJProvider : GenericAiProvider
{
    private readonly Random _temperatureRandom = new(DateTime.Now.Millisecond);

    public GptJProvider(IOptionsMonitor<AiSettings> settings, 
                        IHttpClientFactory factory,
                        ILogger<GptJProvider> logger,
                        IBotticelliBusAgent bus) : base(settings, factory, logger,
                                                        bus)
    {
    }

    public override async Task SendAsync(AiMessage message, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(message.Body)) 
            throw new AiException($"{nameof(SendAsync)}() body is null or empty!");

        try
        {
            _logger.LogDebug($"{nameof(SendAsync)}({message.ChatId}) started");

            using var client = _factory.CreateClient();

            client.BaseAddress = new Uri(_settings.CurrentValue.Url);

            var content = JsonContent.Create(new GptJMessage
            {
                Text = message.Body,
                GenerateTokensLimit = 50 + (int)(message.Body.Length * 1.5),
                TopP = 0.5,
                TopK = 0,
                Temperature = (_temperatureRandom.Next(0, 900) + 100) / 1000.0
            });

            _logger.LogDebug($"{nameof(SendAsync)}({message.ChatId}) content: {content.Value}");

            var response = await client.PostAsync( Url.Combine($"{_settings.CurrentValue.Url}", "generate"), 
                              content, token);

            if (response.IsSuccessStatusCode)
                await _bus.SendResponse(new SendMessageResponse(Guid.NewGuid().ToString())
                                    {
                                        Message = new Shared.ValueObjects.Message(Guid.NewGuid().ToString())
                                        {
                                            ChatId = message.ChatId,
                                            Subject = message.Subject,
                                            Body = await response.Content.ReadAsStringAsync(token),
                                            Attachments = null,
                                            From = null,
                                            ForwardFrom = null
                                        } 
                                    },
                                    token);
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

    public override string AiName => "gptj";
}