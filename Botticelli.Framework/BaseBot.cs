using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Framework;

public abstract class BaseBot<T> : IBot
    where T : BaseBot<T>
{
    private readonly IList<IAdminResponseProcessor> _adminProcessors = new List<IAdminResponseProcessor>();
    private readonly IList<IClientResponseProcessor> _clientProcessors = new List<IClientResponseProcessor>();

    private readonly ManualResetEventSlim _startEventSlim = new(false);

    public BaseBot()
    {
        ProcessAsync();
    }

    public async Task<PingResponse> PingAsync(PingRequest request)
    {
        return PingResponse.GetInstance(request.Uid);
    }

    public void AddAdminEventProcessor(IAdminResponseProcessor responseProcessor)
    {
        _adminProcessors.Add(responseProcessor);
    }

    public void AddClientEventProcessor(IClientResponseProcessor responseProcessor)
    {
        _clientProcessors.Add(responseProcessor);
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

    public abstract Task<SendMessageResponse> SendAsync(SendMessageRequest request);

    /// <summary>
    ///     Processes responses
    /// </summary>
    /// <param name="response"></param>
    protected void ProcessAsync(BaseResponse response)
    {
        while (true)
        {
            var clientTasks = _clientProcessors.Select(p => ProcessForTask(p, response));
            var adminTasks = _adminProcessors.Select(p => ProcessForTask(p, response));
            var allTasks = clientTasks.Union(adminTasks);

            Task.WhenAll(allTasks);
        }
    }


    private Task ProcessForTask(IResponseProcessor processor, BaseResponse response)
    {
        return Task.Run(() =>
        {
            _startEventSlim.Wait();
            processor.ProcessAsync(response);
        });
    }
}