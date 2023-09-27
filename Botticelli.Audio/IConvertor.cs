namespace Botticelli.Audio;

/// <summary>
/// Converts to different audio formats
/// </summary>
public interface IConvertor
{
    /// <summary>
    /// Converts an input stream
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tgtParams"></param>
    /// <returns></returns>
    public byte[] Convert(Stream input, AudioInfo tgtParams);
    /// <summary>
    /// Converts an input byte array
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tgtParams"></param>
    /// <returns></returns>
    public byte[] Convert(byte[] input, AudioInfo tgtParams);
}