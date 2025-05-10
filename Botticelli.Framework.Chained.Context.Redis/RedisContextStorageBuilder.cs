using System.Configuration;
using Botticelli.Framework.Chained.Context.Settings;

namespace Botticelli.Framework.Chained.Context.Redis;

public class RedisContextStorageBuilder<TKey, TValue> : IContextStorageBuilder<RedisStorage<TKey, TValue>, TKey, TValue>
    where TKey : notnull 
    where TValue : class
{
    private string? _connectionString;
    
    public RedisContextStorageBuilder<TKey, TValue> AddConnectionString(string connectionString)
    {
         _connectionString = connectionString;
         
         return this;
    }

    public RedisStorage<TKey, TValue> Build()
    {
        if (_connectionString != null)
            return new(_connectionString);
        
        throw new ConfigurationErrorsException("No connection string for redis was given!");
    }
}