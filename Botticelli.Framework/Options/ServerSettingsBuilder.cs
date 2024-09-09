namespace Botticelli.Framework.Options;

public class ServerSettingsBuilder<T>
        where T : ServerSettings, new()
{
    private readonly T _settings = new();

    public ServerSettingsBuilder<T> Set(Action<T> func)
    {
        func(_settings);

        return this;
    }

    public T Build() => _settings;
}