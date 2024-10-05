using System.Globalization;
using Botticelli.BotData.Entities.Bot;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Bot.Data;

public class BotInfoContext : DbContext
{
    // public BotInfoContext() : base((new DbContextOptionsBuilder<BotInfoContext>().UseSqlite("Data Source=database.db")).Options)
    // {
    //
    // }
    
    public BotInfoContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<BotData.Entities.Bot.BotData> BotInfos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<BotData.Entities.Bot.BotData>();
}