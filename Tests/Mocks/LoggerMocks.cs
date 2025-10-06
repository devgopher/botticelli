using Microsoft.Extensions.Logging;

namespace Mocks;

public static class LoggerMocks
{
    public static ILogger<T> CreateConsoleLogger<T>()
    {
        return LoggerFactory.Create(o => o.AddConsole())
                            .CreateLogger<T>();
    }
}