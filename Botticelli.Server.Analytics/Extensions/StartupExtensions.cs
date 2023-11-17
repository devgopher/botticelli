using Microsoft.EntityFrameworkCore;

namespace Botticelli.Server.Analytics.Extensions;

public static class StartupExtensions
{
    public static void ApplyMigrations<TContext>(this WebApplicationBuilder webApplicationBuilder)
        where TContext : DbContext
    {
        using var scope = webApplicationBuilder.Services
            .BuildServiceProvider()
            .CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<TContext>();
        var pendingMigrations = db.Database.GetPendingMigrations();

        if (pendingMigrations.Any()) db.Database.Migrate();
    }
}