using Botticelli.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Server.Data;

public class BotInfoContext : DbContext
{
    public BotInfoContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<BotInfo> BotInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BotInfo>();
    }
}