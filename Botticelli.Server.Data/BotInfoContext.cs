using Botticelli.Server.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Server.Data;

public class BotInfoContext : DbContext
{
    public BotInfoContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<BotInfo> BotInfos { get; set; }
    public DbSet<IdentityRole> ApplicationRoles { get; set; }
    public DbSet<IdentityUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BotInfo>();
    }
}