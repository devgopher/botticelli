namespace Botticelli.Bot.Data.Settings;

/// <summary>
/// Bot data access layer settings
/// </summary>
public interface IDataAccessSettings
{
    string ConnectionString { get; set; }
}