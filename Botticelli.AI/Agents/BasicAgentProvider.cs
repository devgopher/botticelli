using System.Net.Http.Headers;
using Botticelli.AI.Message;
using Botticelli.AI.Settings;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Responses;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.Agents;

/// <summary>
/// Represents a basic implementation of an agent provider that handles communication 
/// with AI agents. This class serves as an abstract base for specific agent providers,
/// encapsulating common functionalities such as message validation, logging, and settings management.
/// </summary>
public abstract class BasicAgentProvider<TSettings>(
    IOptions<TSettings> settings,
    IHttpClientFactory factory,
    ILogger logger,
    IBusClient bus,
    IValidator<AiMessage> messageValidator)
    : IAgentProvider
    where TSettings : AgentSettings
{
    /// <summary>
    /// Sends an AI message asynchronously to the designated AI agent for processing.
    /// </summary>
    public async Task SendAsync(AiMessage inputMessage, CancellationToken token)

    {
        await ValidateMessage(inputMessage, token);

        try
        {
            logger.LogDebug("{SendAsyncName}({MessageChatIds}) started", nameof(SendAsync), inputMessage.ChatIds);

            using var client = GetClient();

            var response = await GetAgentResponse(inputMessage, token, client);

            if (response.IsSuccessStatusCode)
            {
                await ProcessAgentResponse(inputMessage, token, response);
            }
            else
            {
                var reason = await response.Content.ReadAsStringAsync(token);
                await SendErrorGptResponse(inputMessage, reason, token);
            }

            logger.LogDebug("{SendAsyncName}({MessageChatIds}) finished", nameof(SendAsync), inputMessage.ChatIds);
        }
        catch (Exception ex)
        {
            await SendErrorResponse(inputMessage, token, ex);
        }
    }

    public abstract string AiName { get; }

    protected virtual async Task SendErrorResponse(AiMessage message, CancellationToken token, Exception ex)
    {
        logger.LogError(ex, ex.Message);
        await bus.SendResponse(new SendMessageResponse(message.Uid)
                               {
                                   IsPartial = false,
                                   Message = new Shared.ValueObjects.Message(message.Uid)
                                   {
                                       ChatIds = message.ChatIds,
                                       Subject = message.Subject,
                                       Body = $"Error getting a response from {AiName}!",
                                       Attachments = null,
                                       From = null,
                                       ForwardedFrom = null
                                   }
                               },
                               token);
    }

    protected virtual async Task ValidateMessage(AiMessage message, CancellationToken token)
    {
        logger.LogError($"{nameof(SendAsync)}() body is null or empty!");

        await messageValidator.ValidateAsync(message, token);
    }

    private HttpClient GetClient()
    {
        var client = factory.CreateClient();

        client.BaseAddress = new Uri(settings.Value.Url);
        if (settings.Value.AuthMethod != null)
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(settings.Value.AuthMethod, settings.Value.ApiKey);

        return client;
    }

    protected virtual async Task SendErrorGptResponse(AiMessage message, string reason, CancellationToken token)
    {
        await bus.SendResponse(new SendMessageResponse(message.Uid)
                               {
                                   Message = new Shared.ValueObjects.Message(message.Uid)
                                   {
                                       ChatIds = message.ChatIds,
                                       Subject = message.Subject,
                                       Body = $"Error getting a response from {AiName}: {reason}!",
                                       Attachments = null,
                                       From = null,
                                       ForwardedFrom = null,
                                       ReplyToMessageUid = message.ReplyToMessageUid
                                   }
                               },
                               token);
    }

    protected abstract Task ProcessAgentResponse(AiMessage message,
                                               CancellationToken token,
                                               HttpResponseMessage response);

    protected abstract Task<HttpResponseMessage> GetAgentResponse(AiMessage message,
                                                                CancellationToken token,
                                                                HttpClient client);
}