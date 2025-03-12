using Microsoft.Extensions.Options;

namespace Shared;

public class OptionsMonitorMock<T> : IOptionsMonitor<T>
{
    public OptionsMonitorMock(T value)
    {
        CurrentValue = value;
    }


    public T Get(string? name)
    {
        return CurrentValue;
    }

    public IDisposable? OnChange(Action<T, string?> listener)
    {
        return default;
    }

    public T CurrentValue { get; }
}