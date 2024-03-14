using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Botticelli.AI.AIProvider;
using Botticelli.AI.ChatGpt.Message.ChatGpt;
using Botticelli.AI.ChatGpt.Settings;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Responses;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Botticelli.AI.ChatGpt.Provider;

public class ChatGptProvider : GenericAiProvider<GptSettings>
{
    public ChatGptProvider(IOptions<GptSettings> gptSettings,
        IHttpClientFactory factory,
        ILogger<ChatGptProvider> logger,
        IBusClient bus) : base(gptSettings,
        factory,
        logger,
        bus)
    {
    }

    public override string AiName => "chatgpt";

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

            var content = JsonContent.Create(new ChatGptInputMessage
            {
                Model = Settings.Value.Model,
                Messages = new List<ChatGptMessage>
                {
                    new()
                    {
                        Role = "user",
                        Content = message.Body
                    }
                },
                Temperature = Settings.Value.Temperature,
                Stream = Settings.Value.StreamGeneration
            });

            Logger.LogDebug(
                $"{nameof(SendAsync)}({message.ChatIds}) content: {JsonConvert.SerializeObject(content.Value)}");

            var response = await client.PostAsync(Url.Combine($"{Settings.Value.Url}", "completions"),
                content,
                token);

            if (response.IsSuccessStatusCode)
            {
                var text = new StringBuilder();

                var outStream = await response.Content.ReadAsStreamAsync(token);
                using var sr = new StreamReader(outStream);

                using var reader = TextReader.Synchronized(sr);
                var partText = await reader.ReadLineAsync(token);
                var seqNumber = 0;
                while (partText != null)
                {
                    try
                    {
                        if (Settings.Value.StreamGeneration)
                            partText = partText.Replace("data: ", string.Empty);

                        Logger.LogInformation(
                            $"{nameof(SendAsync)}({message.ChatIds}) part text: {partText}");
                        
                        var part = JsonConvert.DeserializeObject<ChatGptOutputMessage>(partText);

                        
                        text.AppendJoin(' ',
                            part?.Choices?
                                .Select(c => (c.Message ?? c.Delta)?.Content) ?? Array.Empty<string>());
                       
                        var responseMessage = new SendMessageResponse(message.Uid)
                        {
                            IsPartial = Settings.Value.StreamGeneration,
                            SequenceNumber = seqNumber++,
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
                        };
                        
                        await Bus.SendResponse(responseMessage,
                            token);

                        Logger.LogInformation(
                            $"{nameof(SendAsync)}({message.ChatIds}) sent a response message uid '{responseMessage.Uid}', seq number: {responseMessage.SequenceNumber}");

                        if (Settings.Value.StreamGeneration)
                            if (part?.Choices?.Any(
                                    c => c.FinishReason != null ? c.FinishReason.Contains("stop") : false) ==
                                true)
                                break;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, ex.Message);
                    }

                    if (Settings.Value.StreamGeneration)
                        partText = await reader.ReadLineAsync(token);
                    else 
                        break;
                }
            }
            else
            {
                await Bus.SendResponse(new SendMessageResponse(message.Uid)
                    {
                        IsPartial = false,
                        Message = new Shared.ValueObjects.Message(message.Uid)
                        {
                            ChatIds = message.ChatIds,
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

            Logger.LogDebug($"{nameof(SendAsync)}({message.ChatIds}) finished");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            await Bus.SendResponse(new SendMessageResponse(message.Uid)
                {
                    IsPartial = false,
                    Message = new Shared.ValueObjects.Message(message.Uid)
                    {
                        ChatIds = message.ChatIds,
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