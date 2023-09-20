namespace Botticelli.Audio;

public interface IConvertor
{
    public byte[] Convert(Stream input, AudioInfo targetParams);
}