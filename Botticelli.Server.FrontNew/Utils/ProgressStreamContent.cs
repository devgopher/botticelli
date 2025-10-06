using System.Net;

namespace Botticelli.Server.FrontNew.Utils;

public class ProgressStreamContent(Stream content, Action<long, long> progress) : StreamContent(content)
{
    private const int BufferSize = 8192;
    
    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        var totalBytes = Headers.ContentLength ?? -1;
        var buffer = new byte[BufferSize];
        long totalRead = 0;
        int bytesRead;

        var contentStream = await ReadAsStreamAsync();
        contentStream.Seek(0, SeekOrigin.Begin);
        while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
        {
            await stream.WriteAsync(buffer.AsMemory(0, bytesRead));
            totalRead += bytesRead;
            progress?.Invoke(totalRead, totalBytes);
        }
    }
}