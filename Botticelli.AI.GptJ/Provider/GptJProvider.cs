using System.Net.Http.Headers;
using System.Net.Http.Json;
using Botticelli.AI.AIProvider;
using Botticelli.AI.Exceptions;
using Botticelli.AI.GptJ.Message.GptJ;
using Botticelli.AI.GptJ.Settings;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Responses;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.GptJ.Provider;

public class GptJProvider : GenericAiProvider<AiGptSettings>
{
    private readonly Random _temperatureRandom = new(DateTime.Now.Millisecond);

    public GptJProvider(IOptionsSnapshot<AiGptSettings> gptSettings,
        IHttpClientFactory factory,
        ILogger<GptJProvider> logger,
        IBusClient bus) : base(gptSettings,
        factory,
        logger,
        bus)
    {
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

            client.BaseAddress = new Uri(Settings.Value.Url);

            if (!string.IsNullOrWhiteSpace(Settings.Value.ApiKey))
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Settings.Value.ApiKey);

            var content = JsonContent.Create(new GptJInputMessage
            {
                Text = message.Body,
                GenerateTokensLimit = Settings.Value.GenerateTokensLimit,
                TopP = Settings.Value.TopP,
                TopK = Settings.Value.TopK,
                Temperature = Settings.Value.Temperature
            });

            Logger.LogDebug($"{nameof(SendAsync)}({message.ChatIds}) content: {content.Value}");

            var response = await client.PostAsync(Url.Combine($"{Settings.Value.Url}", "generate"),
                content,
                token);

            if (response.IsSuccessStatusCode)
            {
                var outMessage = await response.Content.ReadFromJsonAsync<GptJOutputMessage>(cancellationToken: token);

                await Bus.SendResponse(new SendMessageResponse(message.Uid)
                    {
                        IsPartial = Settings.Value.ExpectPartialResponses,
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