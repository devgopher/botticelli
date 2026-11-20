using Botticelli.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Server.Back.Tests.Helpers;

internal static class TestServerDataContextFactory
{
    public static ServerDataContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<ServerDataContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        var context = new ServerDataContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
