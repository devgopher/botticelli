using Botticelli.Broadcasting.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Broadcasting.Dal;

public class BroadcastingContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MessageStatus>()
                    .HasKey(k => new {k.ChatId, k.MessageId});
    }
    
    public DbSet<Chat> Chats { get; set; }
    public DbSet<MessageCache> MessageCaches { get; set; }
    public DbSet<MessageStatus> MessageStatuses { get; set; }
}