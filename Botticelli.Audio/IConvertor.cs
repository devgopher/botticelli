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
    /// <param name="targetParams"></param>
    /// <returns></returns>
    public byte[] Convert(Stream input, AudioInfo targetParams);
    /// <summary>
    /// Converts an input byte array
    /// </summary>
    /// <param name="input"></param>
    /// <param name="targetParams"></param>
    /// <returns></returns>
    public byte[] Convert(byte[] input, AudioInfo targetParams);
}