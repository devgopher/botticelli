namespace Botticelli.Audio;

public interface IConvertor
{
    public byte[] Convert(Stream input, AudioInfo targetParams);
    public byte[] Convert(byte[] input, AudioInfo targetParams);
}