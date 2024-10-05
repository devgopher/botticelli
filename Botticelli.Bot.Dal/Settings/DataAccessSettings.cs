namespace Botticelli.Bot.Data.Settings;

/// <summary>
/// Bot data access layer settings
/// </summary>
public class DataAccessSettings : IDataAccessSettings
{
    public string ConnectionString { get; set; }
}