namespace Botticelli.Audio;

public interface IConvertor
{
    public byte[] Convert(byte[] input, AudioInfo targetParams);
}