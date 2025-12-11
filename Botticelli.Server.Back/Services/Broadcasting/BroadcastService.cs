using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Bot.Broadcasting;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Server.Back.Services.Broadcasting;

/// <summary>
///     This class is intended for broadcasting
/// </summary>
public class BroadcastService(ServerDataContext context) : IBroadcastService
{
    private readonly ServerDataContext _context = context;

    public async Task BroadcastMessage(Broadcast message)
    {
        _context.BroadcastMessages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Broadcast>> GetMessages(string botId) =>
        await _context.BroadcastMessages
            .Where(m => m.BotId.Equals(botId) && !m.Received)
            .Include(m => m.Attachments)
            .ToArrayAsync();

    public async Task MarkReceived(string botId, string messageId)
    {
        var messages = await _context.BroadcastMessages.Where(bm => bm.BotId == botId && bm.Id == messageId)
            .ToListAsync();

        foreach (var message in messages) message.Received = true;

        _context.UpdateRange(messages);

        await _context.SaveChangesAsync();
    }
}