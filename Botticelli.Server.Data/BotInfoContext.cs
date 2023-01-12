using Botticelli.Server.Data.Entities;
using Botticelli.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Server.Data
{
    public class BotInfoContext : DbContext
    {
        public BotInfoContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
            => modelBuilder.Entity<BotInfo>();

        public DbSet<BotInfo> BotInfos { get; set; }
    }
}