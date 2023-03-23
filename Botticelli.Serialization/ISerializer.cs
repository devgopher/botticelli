using System.Dynamic;

namespace Botticelli.Serialization;

/// <summary>
///     Serializer interface
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISerializer<T>
{
    /// <summary>
    ///     Serialize
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string Serialize(T input);

    /// <summary>
    ///     Serialize to byte array
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public byte[] SerializeToBytes(T input);

    /// <summary>
    ///     Deserialize from stream
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public T? Deserialize(Stream input);


    /// <summary>
    ///     Deserialize from byte array
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public T? Deserialize(byte[] input);


    /// <summary>
    ///     Deserialize from string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public T? Deserialize(string input);
}