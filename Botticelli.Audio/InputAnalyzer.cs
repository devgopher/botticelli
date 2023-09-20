using FileSignatures;
using FileSignatures.Formats;
using NAudio.CoreAudioApi;
using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Botticelli.Audio;

public class InputAnalyzer : IAnalyzer
{
    private readonly FileFormatInspector _fileFormatInspector = new();
    public AudioInfo Analyze(string filePath)
    {
        using var fr = new AudioFileReader(filePath);

        var format = Path.GetExtension(filePath).ToLowerInvariant() switch
        {
            "wav" => AudioFormat.Wav,
            "mp3" => AudioFormat.Mp3,
            "m4a" => AudioFormat.M4a,
            "aac" => AudioFormat.Aac,
            "ogg" => AudioFormat.Ogg,
            _     => AudioFormat.Unknown
        };

        return new AudioInfo
        {
            AudioFormat = format,
            SampleRate = fr.WaveFormat.SampleRate,
            BitsPerSample = fr.WaveFormat.BitsPerSample,
            Channels = fr.WaveFormat.Channels
        };
    }

    public AudioInfo Analyze(Stream input)
    {
        var fileFormat = _fileFormatInspector.DetermineFileFormat(input);
        AudioFormat format;
        WaveStream reader;

        switch (fileFormat.Extension.ToLowerInvariant())
        {
            case "wav":
                format = AudioFormat.Wav;
                reader = new WaveFileReader(input);
                break;
            case "mp3":
                format = AudioFormat.Mp3;
                reader = new Mp3FileReader(input);
                break;
            case "m4a":
            case "aac":
                //format = AudioFormat.M4a;
                //reader = new (input);
                reader = default;
                format = default;
                break;
            case "ogg":
                format = AudioFormat.Ogg;
                reader = new VorbisWaveReader(input);
                break;
            default:
                format = AudioFormat.Unknown;
                reader = default;
                break;
        }

        if (format == AudioFormat.Unknown) throw new InvalidOperationException($"Invalid format!");

        if (reader == default) throw new InvalidOperationException($"Reader is null!");
        
        var bitrate = reader.WaveFormat.SampleRate * reader.WaveFormat.BitsPerSample;

        return new AudioInfo
        {
            AudioFormat = format,
            Bitrate = bitrate
        };
    }
}