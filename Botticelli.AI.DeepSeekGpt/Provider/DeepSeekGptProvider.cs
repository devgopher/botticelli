using System.Net.Http.Headers;
using System.Net.Http.Json;
using Botticelli.AI.AIProvider;
using Botticelli.AI.DeepSeekGpt.Message.DeepSeek;
using Botticelli.AI.DeepSeekGpt.Settings;
using Botticelli.AI.Exceptions;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.DeepSeekGpt.Provider;

public class DeepSeekGptProvider : GenericAiProvider<DeepSeekGptSettings>
{
    private const string SystemRole = "system";
    private const string UserRole = "user";
    private const string Completion = "completions";

    public DeepSeekGptProvider(IOptions<DeepSeekGptSettings> gptSettings,
        IHttpClientFactory factory,
        ILogger<DeepSeekGptProvider> logger,
        IBusClient bus) : base(gptSettings,
        factory,
        logger,
        bus)
    {
    }

    public override string AiName => "deepseek";

    public override async Task SendAsync(AiMessage message, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(message.Body?.Trim()))
        {
            Logger.LogError($"{nameof(SendAsync)}() body is null or empty!");

            await Bus.SendResponse(new SendMessageResponse(message.Uid)
                {
                    IsPartial = Settings.Value.StreamGeneration,
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

            var deepSeekGptMessage = new DeepSeekInputMessage
            {
                Model = Settings.Value.Model,
                MaxTokens = Settings.Value.MaxTokens,
                Messages = new List<DeepSeekInnerInputMessage>
                {
                    new()
                    {
                        Role = SystemRole,
                        Content = Settings.Value.Instruction
                    },
                    new()
                    {
                        Role = UserRole,
                        Content = message.Body
                    }
                }
            };

            deepSeekGptMessage.Messages.AddRange(message.AdditionalMessages?.Select(m => new DeepSeekInnerInputMessage
                                                 {
                                                     Role = UserRole,
                                                     Content = m.Body ?? string.Empty
                                                 }) ??
                                                 new List<DeepSeekInnerInputMessage>());

            var content = JsonContent.Create(deepSeekGptMessage);

            Logger.LogDebug($"{nameof(SendAsync)}({message.ChatIds}) content: {content.Value}");

            var response = await client.PostAsync(Completion,
                content,
                token);

            if (response.IsSuccessStatusCode)
            {
                var outMessage =
                    await response.Content.ReadFromJsonAsync<DeepSeekOutputMessage>(cancellationToken: token);

                if (outMessage == null)
                    throw new AiException($"{nameof(outMessage)} = null!");

                await Bus.SendResponse(new SendMessageResponse(message.Uid)
                    {
                        Message = new Shared.ValueObjects.Message(message.Uid)
                        {
                            ChatIds = message.ChatIds,
                            Subject = message.Subject,
                            Body = string.Join(" ",
                                outMessage.Choices.Select(c => c.DeepSeekMessage?.Content ?? string.Empty)),
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