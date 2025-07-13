using Botticelli.Broadcasting.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Broadcasting.Dal;

public class BroadcastingContext : DbContext
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<MessageCache> MessageCaches { get; set; }
    public DbSet<MessageStatus> MessageStatuses { get; set; }

    public BroadcastingContext()
    {
        
    }

    public BroadcastingContext(DbContextOptions<BroadcastingContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MessageStatus>()
            .HasKey(k => new { k.ChatId, k.MessageId });
    }
}