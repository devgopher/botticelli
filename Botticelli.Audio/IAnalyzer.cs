using System.Runtime.InteropServices;
using NAudio.Wave;

namespace Botticelli.Audio;

public interface IAnalyzer
{
    public AudioInfo Analyze(string filePath);
}

public class InputAnalyzer : IAnalyzer
{
    public AudioInfo Analyze(string filePath)
    {
        using var fr = new AudioFileReader(filePath);

        var bitrate = fr.WaveFormat.SampleRate * fr.WaveFormat.BitsPerSample;

        AudioFormat format = Path.GetExtension(filePath).ToLowerInvariant() switch
        {
            "wav" => AudioFormat.Wav,
            "mp3" => AudioFormat.Mp3,
            "m4a" => AudioFormat.M4a,
            "aac" => AudioFormat.Aac,
            "ogg" => AudioFormat.Ogg,
            _     => AudioFormat.Unknown
        };

        return new AudioInfo()
        {
            AudioFormat = format,
            Bitrate = bitrate
        };
    }
}
