namespace Botticelli.Serialization;

public class JsonSerializerFactory : ISerializerFactory
{
    public ISerializer<T> GetSerializer<T>()
        => new JsonSerializer<T>();
}