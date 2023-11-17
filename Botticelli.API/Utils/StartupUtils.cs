using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.BotBase.Utils;

public static class StartupUtils
{
    public static void ApplyMigrations<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        using var scope = services.BuildServiceProvider().CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<TContext>();
        var pendingMigrations = db.Database.GetPendingMigrations();

        if (pendingMigrations.Any()) db.Database.Migrate();
    }
}