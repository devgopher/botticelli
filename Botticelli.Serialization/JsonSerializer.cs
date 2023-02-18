using System.Text.Json;

namespace Botticelli.Serialization;

/// <inheritdoc />
public sealed class JsonSerializer<T> : ISerializer<T>
{
    /// <inheritdoc />
    public string Serialize(T input) => JsonSerializer.Serialize(input);

    /// <inheritdoc />
    public byte[] SerializeToBytes(T input) => JsonSerializer.SerializeToUtf8Bytes(input);

    /// <inheritdoc />
    public T? Deserialize(Stream input) => JsonSerializer.Deserialize<T>(input);

    /// <inheritdoc />
    public T? Deserialize(byte[] input) => JsonSerializer.Deserialize<T>(input);

    /// <inheritdoc />
    public T? Deserialize(string input) => JsonSerializer.Deserialize<T>(input);
}