namespace Botticelli.Shared.Utils;

public static class StreamUtils
{
    public static Stream ToStream(this byte[] input)
    {
        var stream = new MemoryStream(input);
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }

    public static string FromStreamToString(this Stream input)
    {
        CheckForNull(input);
        
        using var sr = new StreamReader(input);

        return sr.ReadToEnd();
    }
    
    public static byte[] FromStreamToBytes(this Stream input)
    {
        CheckForNull(input);

        using var memoryStream = new MemoryStream();
        input.CopyTo(memoryStream);
        
        return memoryStream.ToArray();
    }
    
    public static async Task<byte[]> FromStreamToBytesAsync(this Stream input)
    {
        CheckForNull(input);

        using var memoryStream = new MemoryStream();
        await input.CopyToAsync(memoryStream);
        
        return memoryStream.ToArray();
    }
    
    private static void CheckForNull(Stream input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input), "Stream cannot be null.");
    }
}