namespace Botticelli.Shared.Constants;

public static class MimeTypeConverter
{
    public static MediaType ConvertMimeTypeToMediaType(string mimeType)
    {
        if (string.IsNullOrEmpty(mimeType)) return MediaType.Unknown;

        switch (mimeType.ToLowerInvariant())
        {
            case var _ when mimeType.StartsWith("audio/"):
            case "application/ogg": return MediaType.Audio;
            case var _ when mimeType.StartsWith("video/"): return MediaType.Video;
            case var _ when mimeType.StartsWith("text/"):  return MediaType.Text;
            case var _ when mimeType.StartsWith("image/"): return MediaType.Image;
            case var _ when mimeType.StartsWith("application/"):
            case var _ when mimeType.StartsWith("multipart/"):
                return MediaType.Document;
            default: return MediaType.Unknown;
        }
    }
}