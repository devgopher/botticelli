using System.Net.Http.Headers;
using System.Net.Http.Json;
using Botticelli.AI.Exceptions;
using Botticelli.AI.Message;
using Botticelli.AI.Message.GptJ;
using Botticelli.AI.Settings;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Responses;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.AIProvider;

public class GptJProvider : GenericAiProvider
{
    private readonly IOptionsMonitor<AiGptSettings> _gptSettings;
    private readonly Random _temperatureRandom = new(DateTime.Now.Millisecond);

    public GptJProvider(IOptionsMonitor<AiGptSettings> gptSettings,
        IHttpClientFactory factory,
        ILogger<GptJProvider> logger,
        IBusClient bus) : base(gptSettings,
        factory,
        logger,
        bus)
    {
        _gptSettings = gptSettings;
    }

    public override string AiName => "gptj";

    public override async Task SendAsync(AiMessage message, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(message.Body))
            throw new AiException($"{nameof(SendAsync)}() body is null or empty!");

        try
        {
            Logger.LogDebug($"{nameof(SendAsync)}({message.ChatIds}) started");

            using var client = Factory.CreateClient();

            client.BaseAddress = new Uri(Settings.CurrentValue.Url);

            if (!string.IsNullOrWhiteSpace(_gptSettings.CurrentValue.ApiKey))
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _gptSettings.CurrentValue.ApiKey);

            var content = JsonContent.Create(new GptJInputMessage
            {
                Text = message.Body,
                GenerateTokensLimit = _gptSettings?.CurrentValue?.GenerateTokensLimit ?? 300,
                TopP = _gptSettings?.CurrentValue?.TopP ?? 0.5,
                TopK = _gptSettings?.CurrentValue?.TopK ?? 0,
                Temperature = _gptSettings?.CurrentValue?.Temperature ??
                              (_temperatureRandom.Next(0, 900) + 100) / 1000.0
            });

            Logger.LogDebug($"{nameof(SendAsync)}({message.ChatIds}) content: {content.Value}");

            var response = await client.PostAsync(Url.Combine($"{Settings.CurrentValue.Url}", "generate"),
                content,
                token);

            if (response.IsSuccessStatusCode)
            {
                var outMessage = await response.Content.ReadFromJsonAsync<GptJOutputMessage>();

                await Bus.SendResponse(new SendMessageResponse(message.Uid)
                    {
                        Message = new Shared.ValueObjects.Message(message.Uid)
                        {
                            ChatIds = message.ChatIds,
                            Subject = message.Subject,
                            Body = outMessage?.Completion,
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
                await Bus.SendResponse(new SendMessageResponse(message.Uid)
                    {
                        Message = new Shared.ValueObjects.Message(message.Uid)
                        {
                            ChatIds = message.ChatIds,
                            Subject = message.Subject,
                            Body = "Error getting a response from Gpt-J!",
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
                        Body = "Error getting a response from Gpt-J!",
                        Attachments = null,
                        From = null,
                        ForwardedFrom = null
                    }
                },
                token);
        }
    }
}