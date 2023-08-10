namespace Botticelli.Framework.Vk.API.Responses;

/// <summary>
/// Update() response
/// </summary>
public class UpdatesResponse
{
    /// <summary>
    /// Offset
    /// </summary>
    public int Ts { get; set; }

    /// <summary>
    /// Updated objects
    /// </summary>
    public IEnumerable<UpdateEvent> Updates { get; set; }
}