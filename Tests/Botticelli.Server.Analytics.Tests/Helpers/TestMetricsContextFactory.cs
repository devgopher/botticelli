using Botticelli.Server.Analytics.Cache;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Botticelli.Server.Analytics.Tests.Helpers;

internal static class TestMetricsContextFactory
{
    public static (MetricsContext Context, Mock<ICacheAccessor> Cache) Create(string? dbName = null)
    {
        var cache = new Mock<ICacheAccessor>();
        var options = new DbContextOptionsBuilder<MetricsContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        var context = new MetricsContext(options, cache.Object);
        return (context, cache);
    }
}
