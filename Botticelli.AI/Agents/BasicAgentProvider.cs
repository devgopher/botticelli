using System.Net.Http.Headers;
using Botticelli.AI.Agents.Models;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Responses;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Botticelli.AI.Agents;

/// <summary>
/// Represents a basic implementation of an agent provider that handles communication 
/// with AI agents. This class serves as an abstract base for specific agent providers,
/// encapsulating common functionalities such as message validation, logging, and settings management.
/// </summary>
public abstract class BasicAgentProvider(
    HttpClient client,
    ILogger logger,
    IBusClient bus,
    IValidator<AiMessage> messageValidator)
    : IAgentProvider
{
    public HttpClient Client { get; } = client;

    /// <summary>
    /// Sends an AI message asynchronously to the designated AI agent for processing.
    /// </summary>
    public async Task SendAsync(AiMessage inputMessage, CancellationToken token)
    {
        await ValidateMessage(inputMessage, token);

        try
        {
            logger.LogDebug("{SendAsyncName}({MessageChatIds}) started", nameof(SendAsync), inputMessage.ChatIds);

            var response = await GetAgentResponse(inputMessage, token);

            var result = inputMessage.Copy();

            result.Body = response.Output.FirstOrDefault(o => o.Content != null && o.Content.Count != 0)
                ?.Content
                ?.FirstOrDefault(c => !string.IsNullOrEmpty(c.Text))
                ?.Text;
            
            result.CallbackData = null;
            result.Attachments = null;

            await bus.SendResponse(new SendMessageResponse(inputMessage.Uid)
            {
                MessageUid = inputMessage.Uid,
                MessageSentStatus = MessageSentStatus.Ok,
                Message = result
            }, token);

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
                                       Body = $"Error getting a response from {AiName}!"
                                   }
                               },
                               token);
    }

    protected virtual async Task ValidateMessage(AiMessage message, CancellationToken token)
    {
        logger.LogError($"{nameof(SendAsync)}() body is null or empty!");

        await messageValidator.ValidateAsync(message, token);
    }


    protected abstract Task<AssistantResponse> GetAgentResponse(AiMessage message, CancellationToken token);
}