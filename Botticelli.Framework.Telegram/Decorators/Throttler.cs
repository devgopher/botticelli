namespace Botticelli.Framework.Telegram.Decorators;

public class Throttler
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(5);
    private DateTime _prevDt = DateTime.MinValue;
    
    public Task Throttle(CancellationToken ct)
    {
        var diff = DateTime.UtcNow - _prevDt;
        if (diff < Delay)
            return Task.Delay(Delay - diff, ct);

        _prevDt = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}