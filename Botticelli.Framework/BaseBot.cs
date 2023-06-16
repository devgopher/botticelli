using Botticelli.Framework.Events;
using Botticelli.Framework.SendOptions;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework;

/// <summary>
///     A base class for bot
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseBot<T> : IBot<T>
        where T : BaseBot<T>
{
    public BaseBot(ILogger logger)
    {
        _logger = logger;
        IsStarted = false;
    }

    public delegate void StartedEventHandler(object sender, StartedBotEventArgs e);

    public delegate void StoppedEventHandler(object sender, StoppedBotEventArgs e);

    public delegate void MsgSentEventHandler(object sender, MessageSentBotEventArgs e);

    public delegate void MsgReceivedEventHandler(object sender, MessageReceivedBotEventArgs e);

    public delegate void MsgRemovedEventHandler(object sender, MessageRemovedBotEventArgs e);

    public delegate void MessengerSpecificEventHandler(object sender, MessengerSpecificBotEventArgs<T> e);

    public event StartedEventHandler Started;
    public event StoppedEventHandler Stopped;
    public abstract event MsgReceivedEventHandler MessageReceived;
    public abstract event MsgRemovedEventHandler MessageRemoved;
    public abstract event MessengerSpecificEventHandler MessengerSpecificEvent;
    protected readonly ILogger _logger;

    protected bool IsStarted { get; set; }

    public async Task<PingResponse> PingAsync(PingRequest request) => PingResponse.GetInstance(request.Uid);

    public virtual async Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token)
    {
        StartBotResponse response;

        try
        {
            response = StartBotResponse.GetInstance(request.Uid, string.Empty, AdminCommandStatus.Ok);
        }
        catch (Exception ex)
        {
            response = StartBotResponse.GetInstance(request.Uid, $"Error: {ex.Message}", AdminCommandStatus.Fail);
        }

        Started?.Invoke(this, new StartedBotEventArgs());

        return response;
    }

    public virtual async Task<StopBotResponse> StopBotAsync(StopBotRequest request, CancellationToken token)
    {
        StopBotResponse response;

        try
        {
            response = StopBotResponse.GetInstance(request.Uid, string.Empty, AdminCommandStatus.Ok);
        }
        catch (Exception ex)
        {
            response = StopBotResponse.GetInstance(request.Uid, $"Error: {ex.Message}", AdminCommandStatus.Fail);
        }

        IsStarted = false;

        Stopped?.Invoke(this, new StoppedBotEventArgs());

        return response;
    }

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
    public abstract Task<SendMessageResponse> SendMessageAsync<TSendOptions>(SendMessageRequest request,
                                                                             SendOptionsBuilder<TSendOptions> optionsBuilder,
                                                                             CancellationToken token) 
            where TSendOptions : class;

    public abstract Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request, CancellationToken token);

    public abstract BotType Type { get; }
}