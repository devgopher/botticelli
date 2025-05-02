using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Options;

public class BotDataSettingsBuilder<T>
    where T : BotDataSettings, new()
{
    private T? _settings;

    public static BotDataSettingsBuilder<T> Instance() => new();


    public void Set(T? settings)
    {
        _settings = settings;
    }

    public BotDataSettingsBuilder<T> Set(Action<T> func)
    {
        func(_settings);

        return this;
    }

    public T? Build()
    {
        return _settings;
    }
}