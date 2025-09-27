using System.Net;
using System.Net.Http.Json;
using Botticelli.Broadcasting.Dal;
using Botticelli.Broadcasting.Settings;
using Botticelli.Framework;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Flurl.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Botticelli.Broadcasting;

/// <summary>
/// Broadcaster is a service that polls an admin API for new messages
/// to send to various chat clients. It implements IHostedService to manage
/// the lifecycle of the service within a hosted environment.
/// </summary>
/// <typeparam name="TBot">The type of bot that implements IBot interface.</typeparam>
public class BroadcastReceiver<TBot> : IHostedService
    where TBot : BaseBot, IBot<TBot>
{
    private readonly IBot _bot;
    private readonly BroadcastingContext _context;
    private readonly TimeSpan _longPollTimeout = TimeSpan.FromSeconds(30);
    private readonly TimeSpan _broadcastReceivedTimeout = TimeSpan.FromSeconds(10);
    private readonly TimeSpan _retryPause = TimeSpan.FromMilliseconds(150);
    private const int RetryCount = 3;
    private readonly BroadcastingSettings _settings;
    private readonly ILogger<BroadcastReceiver<TBot>> _logger;
    public CancellationTokenSource CancellationTokenSource { get; private set; }

    public BroadcastReceiver(IBot bot,
                             BroadcastingContext context,
                             BroadcastingSettings settings, 
                             ILogger<BroadcastReceiver<TBot>> logger)
    {
        _bot = bot;
        _settings = settings;
        _logger = logger;
        _context = context;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        // Polls admin API for new messages to send to our chats and adds them to a MessageCache/MessageStatus
        _ = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var updates = await GetUpdates(cancellationToken);

                        if (updates?.Messages == null) continue;

                        foreach (var update in updates.Messages)
                        {
                            // if no chat were specified - broadcast on all chats, we've
                            if (update.ChatIds.Count == 0) update.ChatIds = _context.Chats.Select(x => x.ChatId).ToList();

                            var request = new SendMessageRequest
                            {
                                Message = update
                            };

                            List<string> messageIds = [update.Uid];

                            var response = await _bot.SendMessageAsync(request, cancellationToken);

                            if (response.MessageSentStatus == MessageSentStatus.Ok) 
                                await SendBroadcastReceived(messageIds, cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }

                    await Task.Delay(_retryPause, cancellationToken);
                }
            },
            cancellationToken);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        CancellationTokenSource.Cancel(false);
        
        return Task.CompletedTask;
    }

    private async Task<GetBroadCastMessagesResponse?> GetUpdates(CancellationToken cancellationToken)
    {
        var updatesResponse = await $"{_settings.ServerUri}/bot/client/GetBroadcast"
            .WithTimeout(_longPollTimeout)
            .PostJsonAsync(new GetBroadCastMessagesRequest
            {
                BotId = _settings.BotId
            }, cancellationToken: cancellationToken);

        if (!updatesResponse.ResponseMessage.IsSuccessStatusCode)
            return null;

        return await updatesResponse.ResponseMessage.Content
            .ReadFromJsonAsync<GetBroadCastMessagesResponse>(
                cancellationToken);
    }

    private async Task<BroadCastMessagesReceivedResponse?> SendBroadcastReceived(List<string> chatIds, CancellationToken cancellationToken)
    {
        var response = Policy
            .Handle<HttpRequestException>() // Handle network-related exceptions
            .OrResult<IFlurlResponse>(r => !r.ResponseMessage.IsSuccessStatusCode) // Handle non-success status codes
            .WaitAndRetryAsync(RetryCount, i => _retryPause.Multiply(10 * i))
            .ExecuteAsync(async () => await $"{_settings.ServerUri}/bot/client/BroadcastReceived"
                .WithTimeout(_broadcastReceivedTimeout)
                .PostJsonAsync(new BroadCastMessagesReceivedRequest
                    {
                        BotId = _settings.BotId,
                        MessageIds = chatIds.ToArray()
                    },
                    cancellationToken: cancellationToken));

        return await response.Result
            .ResponseMessage
            .Content
            .ReadFromJsonAsync<BroadCastMessagesReceivedResponse>(cancellationToken);
    }
}