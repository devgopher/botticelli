using Microsoft.Extensions.Options;

namespace Botticelli.Framework.Vk.Tests;

internal class OptionsMonitorMock<T> : IOptionsMonitor<T>
{
    public OptionsMonitorMock(T value) => CurrentValue = value;


    public T Get(string? name) => CurrentValue;

    public IDisposable? OnChange(Action<T, string?> listener) => default;

    public T CurrentValue { get; }
}