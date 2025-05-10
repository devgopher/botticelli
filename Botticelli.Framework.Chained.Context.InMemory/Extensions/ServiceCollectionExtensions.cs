using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Chained.Context.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChainedInMemoryStorage<TKey, TValue>(this IServiceCollection services, Action<InMemoryContextStorageBuilder<TKey, TValue>> builderFunc)
        where TKey : notnull
    {
        InMemoryContextStorageBuilder<TKey, TValue> builder = new();
        builderFunc(builder);
        
        services.AddSingleton<IStorage<TKey, TValue>, InMemoryStorage<TKey, TValue>>(_ => builder.Build());
        
        return services;
    }
}