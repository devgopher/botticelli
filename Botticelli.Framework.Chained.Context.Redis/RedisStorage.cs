using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace Botticelli.Framework.Chained.Context.Redis;

/// <summary>
///     RedisStorage storage
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class RedisStorage<TKey, TValue> : IStorage<TKey, TValue>
    where TKey : notnull where TValue : class
{
    private readonly IDatabase _database;
    
    public RedisStorage(string connectionString)
    {
        var redis = ConnectionMultiplexer.Connect(connectionString);
        _database = redis.GetDatabase();
    }

    public bool ContainsKey(TKey key) => _database.KeyExists(key.ToString());

    public void Add(TKey key, TValue? value)
    {
        _database.StringSet(key.ToString(),
            value is not string ? JsonSerializer.Serialize(value?.ToString()) : value.ToString());
    }

    public bool Remove(TKey key) => _database.KeyDelete(key.ToString());

    public bool TryGetValue(TKey key, out TValue? value)
    {
        value = default!;
        
        if (!ContainsKey(key))
            return false;

        InnerGet(key, ref value);
        
        return true;
    }

    private void InnerGet(TKey key, [DisallowNull] ref TValue? value)
    {
        if (value is string)
        {
            value = _database.StringGet(key.ToString()) as TValue;
        }
        else
        {
            var text = _database.StringGet(key.ToString());
            
            if (text is { HasValue: true })
                value = JsonSerializer.Deserialize<TValue>(text!);
        }
    }

    public TValue? this[TKey key]
    {
        get
        {  
            if (!ContainsKey(key))
                return null;
            
            TValue? value = default!;
            
            InnerGet(key, ref value);
            
            return value;
        }
        set => Add(key, value);
    }
}