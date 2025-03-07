using Botticelli.Auth.Data.Extensions;
using Botticelli.Auth.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Auth.Data;

/// <summary>
/// Botticelli.Auth.Sample.Telegram data context
/// </summary>
/// <param name="options"></param>
public class AuthDefaultDbContext(DbContextOptions<AuthDefaultDbContext> options) : DbContext(options)
{
    private const string Schema = "Botticelli.Auth.Sample.Telegram";
    private const string AdminUserId = "d9887829-61a7-4947-9eb6-7faa66363f08";
    private const string GuestUserId = "9947e363-4255-408d-b277-33402b9f07a1";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.Entity<BotUserRole>()
                    .HasData(new BotUserRole
                             {
                                 Id = Guid.Parse(AdminUserId),
                                 Description = "Bot users administrator",
                                 IsSuperUser = true,
                                 RoleName = DefaultRoles.Admin
                             },
                             new BotUserRole
                             {
                                 Id = Guid.Parse(GuestUserId),
                                 Description = "A default user for guest",
                                 IsSuperUser = false,
                                 RoleName = DefaultRoles.Guest
                             },
                             new BotUserRole
                             {
                                 Id = Guid.Parse(GuestUserId),
                                 Description = "A default authorized user role",
                                 IsSuperUser = false,
                                 RoleName = DefaultRoles.User
                             });
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddAuthenticationModels();
    }
    
    private DbSet<BotUser> BotUsers { get; set; }
    private DbSet<BotUserRole> BotUserRoles { get; set; }
    private DbSet<AccessHistory<BotUser>> AccessHistory { get; set; }
}