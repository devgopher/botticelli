using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Shared;

public static class LoggerMocks
{
    public static ILogger<T> CreateConsoleLogger<T>()
    {
        return LoggerFactory.Create(o => o.AddConsole())
                            .CreateLogger<T>();
    }
}