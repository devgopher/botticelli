using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Vk.Tests;

internal static class Utils {
    public static ILogger<T> CreateConsoleLogger<T>()
    {
        return LoggerFactory.Create(o => o.AddConsole())
                            .CreateLogger<T>();
    }
}