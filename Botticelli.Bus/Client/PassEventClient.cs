using Botticelli.Bot.Interfaces.Client;
using Botticelli.Bus.None.Bus;
using Botticelli.Shared.API.Client.Requests;

namespace Botticelli.Bus.None.Client;

public class PassEventClient : IEventBusClient
{
    private bool _startedFlag = true;
    private readonly Task _workerTask;

    public event IEventBusClient.BusEventHandler OnReceived;

    public PassEventClient()
    {
        _workerTask = Task.Run(Process);
    }

    public void Send(SendMessageRequest request)
        => NoneBus.SendMessageRequests.Enqueue(request);

    private void Process()
    {
        const int pause = 50;
        while (_startedFlag)
        {
            if (NoneBus.SendMessageResponses.TryDequeue(out var message))
                OnReceived?.Invoke(this, message);

            Thread.Sleep(pause);
        }
    }

    public void Dispose()
    {
        _startedFlag = false;
        _workerTask.Wait(5000);
    }
}