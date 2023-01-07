using Botticelli.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.DataLayer.Context
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<Chat>();

        public DbSet<Chat> Chats { get; set; }
    }
}
