using System.Net;

namespace Botticelli.Server.FrontNew.Utils;

public class ProgressStreamContent : StreamContent
{
    private readonly Action<long, long> _progress;

    public ProgressStreamContent(Stream content, Action<long, long> progress) : base(content)
    {
        _progress = progress;
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
    {
        var totalBytes = Headers.ContentLength ?? -1;
        var buffer = new byte[8192];
        long totalRead = 0;
        int bytesRead;

        var contentStream = await ReadAsStreamAsync();
        contentStream.Seek(0, SeekOrigin.Begin);
        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await stream.WriteAsync(buffer, 0, bytesRead);
            totalRead += bytesRead;
            _progress?.Invoke(totalRead, totalBytes);
        }
    }
}