namespace Botticelli.Framework.Options;

public class BotOptionsBuilder<T>
    where T : BotSettings, new()
{
    private readonly T _settings = new();

    public BotOptionsBuilder<T> Set(Func<T> func)
    {
        func();
        return this;
    }

    public T Build()
    {
        return _settings;
    }
}