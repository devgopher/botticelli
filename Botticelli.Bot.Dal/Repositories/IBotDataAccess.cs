using Botticelli.Bot.Data.Entities.Bot;

namespace Botticelli.Bot.Data.Repositories;

public interface IBotDataAccess
{
    /// <summary>
    /// Sets or updates bot data
    /// </summary>
    /// <param name="context"></param>
    public void SetData(BotData context);
    
    
    /// <summary>
    /// Gets bot data
    /// </summary>
    /// <returns></returns>
    public BotData? GetData();
}