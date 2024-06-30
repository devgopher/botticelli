using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.LoadTests.Receiver.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoadTesting<TBot>(this IServiceCollection services)
    {
        services.AddScoped<ILoadTestGate, LoadTestGate>();

        return services;
    }
}