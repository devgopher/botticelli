using Botticelli.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.DataLayer.Context;

public class BotticelliContext : DbContext
{
    public BotticelliContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<Chat>();
}