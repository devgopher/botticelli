using Botticelli.Auth.Data;
using Botticelli.Auth.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Auth.Tests.Helpers;

internal static class AuthDbFactory
{
    public static AuthDefaultDbContext Create(string? name = null)
    {
        var options = new DbContextOptionsBuilder<AuthDefaultDbContext>()
            .UseInMemoryDatabase(name ?? Guid.NewGuid().ToString())
            .Options;

        var ctx = new AuthDefaultDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    public static async Task SeedUserAsync(AuthDefaultDbContext ctx, string userId, bool isActive = true)
    {
        var roleId = Guid.Parse("9947e363-4255-408d-b277-33402b9f07a1"); // Guest role from seed
        ctx.Set<BotUser>().Add(new BotUser
        {
            UserId = userId,
            UserName = userId,
            RoleId = roleId,
            IsActive = isActive
        });
        // Also seed a guest user looked up by role name quirk
        if (ctx.Set<BotUser>().All(u => u.UserId != "Guest"))
        {
            ctx.Set<BotUser>().Add(new BotUser
            {
                UserId = "Guest",
                UserName = "Guest",
                RoleId = roleId,
                IsActive = true
            });
        }

        await ctx.SaveChangesAsync();
    }
}
