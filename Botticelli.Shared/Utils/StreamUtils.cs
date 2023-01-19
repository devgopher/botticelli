namespace Botticelli.Shared.Utils;

public static class StreamUtils
{
    public static Stream ToStream(this byte[] input)
    {
        var stream = new MemoryStream(input);
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }
}