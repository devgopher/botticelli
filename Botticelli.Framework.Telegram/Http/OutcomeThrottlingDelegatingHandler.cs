namespace Botticelli.Framework.Telegram.Http;

public class OutcomeThrottlingDelegatingHandler() : DelegatingHandler(new HttpClientHandler())
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(3);
    private static readonly TimeSpan MaxDeviation = TimeSpan.FromSeconds(1);
    private readonly Random _random = Random.Shared;
    private readonly SemaphoreSlim _throttler = new(1, 1);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var randComponent =
            TimeSpan.FromMilliseconds(_random.Next((int)-MaxDeviation.TotalMilliseconds, (int)MaxDeviation.TotalMilliseconds));
        var sumDelay = Delay + randComponent;
        
        await _throttler.WaitAsync(cancellationToken);
        await Task.Delay(sumDelay, cancellationToken);
        
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            _throttler.Release();
        }
    }
}