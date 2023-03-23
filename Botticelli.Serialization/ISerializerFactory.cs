namespace Botticelli.Serialization;

/// <summary>
///     Serializer factory interface
/// </summary>
public interface ISerializerFactory
{
    public ISerializer<T> GetSerializer<T>();
}