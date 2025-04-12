namespace Botticelli.Framework.Options;

public class BotDataSettingsBuilder<T>
    where T : BotDataSettings
{
    private T _settings = new();

    public void Set(T settings)
    {
        _settings = settings;
    }

    public BotDataSettingsBuilder<T> Set(Action<T> func)
    {
        func(_settings);

        return this;
    }

    public T Build()
    {
        return _settings;
    }
}