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
    public DbSet<IdentityRole<string>> ApplicationRoles { get; set; }
    public DbSet<IdentityUserRole<string>> ApplicationUserRoles { get; set; }
    public DbSet<IdentityUser<string>> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BotInfo>();
        modelBuilder.Entity<IdentityRole<string>>()
                    .HasKey(k => k.Id);
        modelBuilder.Entity<IdentityUserRole<string>>()
                    .HasKey(k => new {k.UserId, k.RoleId});
        modelBuilder.Entity<IdentityUser<string>>()
                    .HasKey(k => k.Id);
    }
}