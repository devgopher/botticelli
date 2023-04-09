using LiteDB;

namespace BotDataSecureStorage.Entities;

/// <summary>
///     Bot additional data
/// </summary>
public class BotData
{
    /// <summary>
    ///     botticelli bot id
    /// </summary>
    [BsonId]
    public string? Id { get; set; }

    /// <summary>
    ///     Bot data
    /// </summary>
    public string[]? Data { get; set; }
}