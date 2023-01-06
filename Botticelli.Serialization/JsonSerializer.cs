using System.Text.Json;

namespace Botticelli.Serialization;

/// <inheritdoc />
public sealed class JsonSerializer<T> : ISerializer<T>
{
    /// <inheritdoc />
    public string Serialize(T input)
    {
        return JsonSerializer.Serialize(input);
    }

    /// <inheritdoc />
    public byte[] SerializeToBytes(T input)
    {
        return JsonSerializer.SerializeToUtf8Bytes(input);
    }

    /// <inheritdoc />
    public T? Deserialize(Stream input)
    {
        return JsonSerializer.Deserialize<T>(input);
    }

    /// <inheritdoc />
    public T? Deserialize(byte[] input)
    {
        return JsonSerializer.Deserialize<T>(input);
    }

    /// <inheritdoc />
    public T? Deserialize(string input)
    {
        return JsonSerializer.Deserialize<T>(input);
    }
}