using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Bot.Broadcasting;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Server.Back.Services.Broadcasting;

/// <summary>
///     This class is intended for broadcasting
/// </summary>
public class BroadcastService(ServerDataContext context) : IBroadcastService
{
    public async Task BroadcastMessage(Broadcast message)
    {
        context.BroadcastMessages.Add(message);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Broadcast>> GetMessages(string botId)
        => await context.BroadcastMessages.Where(m => m.BotId.Equals(botId)).ToArrayAsync();

    public async Task MarkReceived(string botId, string messageId)
    {
        var messages = context.BroadcastMessages.Where(bm => bm.BotId == botId && bm.Id == messageId)
                              .ToList();

        foreach (var message in messages)
        {
            message.Received = true;
        }
        
        context.UpdateRange(messages);
        
        await context.SaveChangesAsync();
    }
    
    
    public async Task<List<Broadcast>> GetBroadcasts(string botId)
    {
        var broadcasts = context.BroadcastMessages.Where(x => x.BotId == botId && !x.Sent && !x.Received).ToList();

        return broadcasts;
    }

    public async Task MarkAsReceived(string messageId)
    {
        var broadcast = context.BroadcastMessages.FirstOrDefault(x => x.Id == messageId);
        if (broadcast == null) return;

        broadcast.Received = true;

        context.Update(broadcast);

        await context.SaveChangesAsync();
    }
}