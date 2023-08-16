using Microsoft.Extensions.Options;

namespace Botticelli.Framework.Vk.Tests;

internal class OptionsMonitorMock<T> : IOptionsMonitor<T>
{
    private T _value;

    public OptionsMonitorMock(T value) => _value = value;


    public T Get(string? name) => _value;

    public IDisposable? OnChange(Action<T, string?> listener) => default;

    public T CurrentValue => _value;
}