using System.Net;
using System.Net.Http.Json;
using Botticelli.Broadcasting.Dal;
using Botticelli.Broadcasting.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;

namespace Botticelli.Broadcasting;

/// <summary>
///     Long poll for broadcast messages
/// </summary>
/// <typeparam name="TBot"></typeparam>
public class BroadcastReceiver<TBot> : IHostedService
    where TBot : class, IBot<TBot>
{
    private readonly TBot _bot;
    private readonly BroadcastingContext _context;
    private readonly TimeSpan _longPollTimeout = TimeSpan.FromSeconds(30);
    private readonly TimeSpan _retryPause = TimeSpan.FromMilliseconds(150);
    private readonly IServiceScope _scope;
    private readonly IOptionsSnapshot<BroadcastingSettings> _settings;


    public BroadcastReceiver(IServiceProvider serviceProvider, IOptionsSnapshot<BroadcastingSettings> settings)
    {
        _settings = settings;
        _scope = serviceProvider.CreateScope();
        _bot = _scope.ServiceProvider.GetRequiredService<TBot>();
        _context = _scope.ServiceProvider.GetRequiredService<BroadcastingContext>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // TODO : polls admin API for new messages to send to our chats and adds them to a MessageCache/MessageStatus
        var updatePolicy = Policy.Handle<FlurlHttpException>(ex =>
                ex.Call.Response.ResponseMessage.StatusCode == HttpStatusCode.RequestTimeout)
            .WaitAndRetryForeverAsync((_, _) => _retryPause);

        await updatePolicy.ExecuteAsync(async () =>
        {
            var updates = await GetUpdates(cancellationToken);

            if (updates?.Messages == null) return updates;

            foreach (var update in updates.Messages)
            {
                // if no chat were specified - broadcast on all chats, we've
                if (update.ChatIds.Count == 0) update.ChatIds = _context.Chats.Select(x => x.ChatId).ToList();

                var request = new SendMessageRequest
                {
                    Message = update
                };

                await _bot.SendMessageAsync(request, cancellationToken);
            }

            return updates;
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _scope.Dispose();

        return Task.CompletedTask;
    }

    private async Task<GetBroadCastMessagesResponse?> GetUpdates(CancellationToken cancellationToken)
    {
        var updatesResponse = await $"{_settings.Value.ServerUri}"
            .WithTimeout(_longPollTimeout)
            .PostJsonAsync(new GetBroadCastMessagesRequest
            {
                BotId = _settings.Value.BotId
            }, cancellationToken: cancellationToken);

        if (!updatesResponse.ResponseMessage.IsSuccessStatusCode)
            return null;

        return await updatesResponse.ResponseMessage.Content
            .ReadFromJsonAsync<GetBroadCastMessagesResponse>(
                cancellationToken);
    }
}