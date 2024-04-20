namespace TelegramMessagingSample;

/// <summary>
///     This hosted service intended for sending messages according to a schedule
/// </summary>
public class TestBotHostedService : IHostedService
{
    public TestBotHostedService()
    {
    }

    public Task StartAsync(CancellationToken token)
    {
        Console.WriteLine("Start sending messages...");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stop sending messages...");

        return Task.CompletedTask;
    }
}