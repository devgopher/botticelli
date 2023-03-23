using System.Collections.Concurrent;

namespace Botticelli.Serialization;

public class JsonSerializerFactory : ISerializerFactory
{
    private readonly ConcurrentDictionary<Type, object> _objects = new();

    public ISerializer<T> GetSerializer<T>()
    {
        if (_objects.Keys.All(t => t != typeof(T)))
            new JsonSerializer<T>();
    }
}