namespace Botticelli.Framework.Options;

public class BotOptionsBuilder<T>
        where T : BotSettings, new()
{
    private readonly T _settings = new();

    public BotOptionsBuilder<T> Set(Action<T> func)
    {
        func(_settings);

        return this;
    }

    public T Build() => _settings;
}