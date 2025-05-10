using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Chained.Context.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChainedStorage<TKey, TValue, TStorage>(this IServiceCollection services)
    where TStorage : class, IStorage<TKey, TValue>
        where TKey : notnull
    {
        services.AddSingleton<IStorage<TKey, TValue>, TStorage>();
        
        return services;
    }
}