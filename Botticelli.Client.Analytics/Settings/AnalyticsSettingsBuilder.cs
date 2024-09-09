namespace Botticelli.Client.Analytics.Settings;

public class AnalyticsSettingsBuilder<T>
        where T : AnalyticsSettings, new()
{
    private readonly T _settings = new();

    public AnalyticsSettingsBuilder<T> Set(Action<T> func)
    {
        func(_settings);

        return this;
    }

    public T Build() => _settings;
}