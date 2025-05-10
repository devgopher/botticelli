using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Chained.Context.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChainedRedisStorage<TKey, TValue>(this IServiceCollection services, Action<RedisContextStorageBuilder<TKey, TValue>> builderFunc)
        where TKey : notnull 
        where TValue : class
    {
        RedisContextStorageBuilder<TKey, TValue> builder = new();
        builderFunc(builder);
        
        services.AddSingleton<IStorage<TKey, TValue>, RedisStorage<TKey, TValue>>(_ => builder.Build());
        
        return services;
    }
}