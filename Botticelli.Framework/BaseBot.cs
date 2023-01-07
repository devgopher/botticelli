using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Framework;

/// <summary>
///     A base class for bot
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseBot<T> : IBot<T>
    where T : BaseBot<T>
{
    private readonly IList<IAdminRequestProcessor> _adminProcessors = new List<IAdminRequestProcessor>();
    private readonly IList<IClientRequestProcessor> _clientProcessors = new List<IClientRequestProcessor>();

    private readonly ManualResetEventSlim _startEventSlim = new(false);

    public async Task<PingResponse> PingAsync(PingRequest request)
    {
        return PingResponse.GetInstance(request.Uid);
    }

    public void AddAdminEventProcessor(IAdminRequestProcessor requestProcessor)
    {
        _adminProcessors.Add(requestProcessor);
    }

    public void AddClientEventProcessor(IClientRequestProcessor requestProcessor)
    {
        _clientProcessors.Add(requestProcessor);
    }

    public async Task<StartBotResponse> StartBotAsync(StartBotRequest request)
    {
        StartBotResponse response;

        try
        {
            // unblock threads
            _startEventSlim.Set();

            response = StartBotResponse.GetInstance(request.Uid, string.Empty, AdminCommandStatus.OK);
        }
        catch (Exception ex)
        {
            response = StartBotResponse.GetInstance(request.Uid, $"Error: {ex.Message}", AdminCommandStatus.FAIL);
        }

        return response;
    }

    public async Task<StopBotResponse> StopBotAsync(StopBotRequest request)
    {
        StopBotResponse response = null;

        try
        {
            // block threads
            _startEventSlim.Reset();

            response = StopBotResponse.GetInstance(request.Uid, string.Empty, AdminCommandStatus.OK);
        }
        catch (Exception ex)
        {
            response = StopBotResponse.GetInstance(request.Uid, $"Error: {ex.Message}", AdminCommandStatus.FAIL);
        }

        return response;
    }

    /// <summary>
    ///     Sends a message
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public abstract Task<SendMessageResponse> SendAsync(SendMessageRequest request);

    /// <summary>
    ///     Processes requests
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    protected void ProcessAsync(BaseRequest request, CancellationToken token)
    {
        while (true)
        {
            if (token is { CanBeCanceled: true, IsCancellationRequested: true })
                break;

            var clientTasks = _clientProcessors.Select(p => ProcessForProcessor(p, request, token));
            var adminTasks = _adminProcessors.Select(p => ProcessForProcessor(p, request, token));
            var allTasks = clientTasks.Union(adminTasks);

            Task.WhenAll(allTasks);
        }
    }

    /// <summary>
    /// Request processing for a particular processor
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private Task ProcessForProcessor(IRequestProcessor processor, BaseRequest request, CancellationToken token)
    {
        return Task.Run(() =>
        {
            _startEventSlim.Wait(token);
            processor.ProcessAsync(request, token);
        }, token);
    }
}