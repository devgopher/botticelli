using Botticelli.Analytics.Shared.Metrics;
using Botticelli.BotBase.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Events;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework;

/// <summary>
///     A base class for bot
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseBot<T> : IBot<T>
    where T : BaseBot<T>, IBot
{
    public delegate void MessengerSpecificEventHandler(object sender, MessengerSpecificBotEventArgs<T> e);

    public delegate void MsgReceivedEventHandler(object sender, MessageReceivedBotEventArgs e);

    public delegate void MsgRemovedEventHandler(object sender, MessageRemovedBotEventArgs e);

    public delegate void MsgSentEventHandler(object sender, MessageSentBotEventArgs e);

    public delegate void StartedEventHandler(object sender, StartedBotEventArgs e);

    public delegate void StoppedEventHandler(object sender, StoppedBotEventArgs e);

    protected readonly ILogger Logger;
    private readonly MetricsProcessor _metrics;

    protected BaseBot(ILogger logger, MetricsProcessor metrics)
    {
        Logger = logger;
        _metrics = metrics;
    }

    public async Task<PingResponse> PingAsync(PingRequest request) => PingResponse.GetInstance(request.Uid);

    public virtual async Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token)
    {
        if (BotStatusKeeper.IsStarted)
            return StartBotResponse.GetInstance(request.Uid, string.Empty, AdminCommandStatus.Ok);

        _metrics.Process(MetricNames.BotStarted, BotDataUtils.GetBotId());

        var result = await InnerStartBotAsync(request, token);

        Started?.Invoke(this, new StartedBotEventArgs());

        return result;
    }

    public virtual async Task<StopBotResponse> StopBotAsync(StopBotRequest request, CancellationToken token)
    {
        _metrics.Process(MetricNames.BotStopped, BotDataUtils.GetBotId());

        if (!BotStatusKeeper.IsStarted)
            return StopBotResponse.GetInstance(request.Uid, string.Empty, AdminCommandStatus.Ok);
        
        var result = await InnerStopBotAsync(request, token);
        
        Stopped?.Invoke(this, new StoppedBotEventArgs());

        return result;
    }

    [Obsolete($"Use {nameof(SetBotContext)}")]
    public abstract Task SetBotKey(string key, CancellationToken token);

    public abstract Task SetBotContext(BotContext context, CancellationToken token);

    /// <summary>
    ///     Sends a message
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken token)
        => SendMessageAsync<object>(request, null, token);


    /// <summary>
    ///     Sends a message
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="optionsBuilder"></param>
    /// <returns></returns>
    public virtual async Task<SendMessageResponse> SendMessageAsync<TSendOptions>(SendMessageRequest request,
        ISendOptionsBuilder<TSendOptions> optionsBuilder,
        CancellationToken token)
        where TSendOptions : class
    {
        _metrics.Process(MetricNames.MessageSent, BotDataUtils.GetBotId());

        return await InnerSendMessageAsync(request, optionsBuilder, token);
    }

    public virtual async Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request,
        CancellationToken token)
    {
        _metrics.Process(MetricNames.MessageRemoved, BotDataUtils.GetBotId());

        return await InnerDeleteMessageAsync(request, token);
    }

    public abstract BotType Type { get; }

    protected abstract Task<StartBotResponse> InnerStartBotAsync(StartBotRequest request, CancellationToken token);

    protected abstract Task<StopBotResponse> InnerStopBotAsync(StopBotRequest request, CancellationToken token);

    public virtual event MsgSentEventHandler MessageSent;
    public virtual event MsgReceivedEventHandler MessageReceived;
    public virtual event MsgRemovedEventHandler MessageRemoved;
    public virtual event MessengerSpecificEventHandler MessengerSpecificEvent;

    protected abstract Task<SendMessageResponse> InnerSendMessageAsync<TSendOptions>(SendMessageRequest request,
        ISendOptionsBuilder<TSendOptions> optionsBuilder,
        CancellationToken token)
        where TSendOptions : class;

    protected abstract Task<RemoveMessageResponse> InnerDeleteMessageAsync(RemoveMessageRequest request,
        CancellationToken token);

    public event StartedEventHandler Started;
    public event StoppedEventHandler Stopped;
}