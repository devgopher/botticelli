using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Exceptions;
using FFMpegCore.Pipes;
using NAudio.Lame;
using NAudio.Vorbis;
using NAudio.Wave;

namespace Botticelli.Audio;

public class UniversalConvertor : IConvertor
{
    private readonly IAnalyzer _analyzer;

    public UniversalConvertor(IAnalyzer analyzer) => _analyzer = analyzer;

    public byte[] Convert(Stream input, AudioInfo targetParams)
    {
        if (targetParams.AudioFormat is AudioFormat.M4a or AudioFormat.Aac or AudioFormat.Opus)
            return ProcessByStreamEncoder(input, targetParams);

        var srcParams = _analyzer.Analyze(input);

        using var resultStream = new MemoryStream();
        using var srcStream = GetSourceWaveStream(input, srcParams);
        using var tgtStream = GetTargetWaveStream(srcStream, targetParams);

        return tgtStream.ToArray();
    }

    private static byte[] ProcessByStreamEncoder(Stream input, AudioInfo tgtParams)
    {
        try
        {
            var codec = string.Empty;
            input.Seek(0, SeekOrigin.Begin);

            switch (tgtParams.AudioFormat)
            {
                case AudioFormat.Ogg:
                    codec = "ogg";
                    break;
                case AudioFormat.Aac:
                case AudioFormat.M4a:
                    codec = "aac";
                    break;
                case AudioFormat.Opus:
                    codec = "opus";
                    break;
                case AudioFormat.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            using var output = new MemoryStream();
            FFMpegArguments
                .FromPipeInput(new StreamPipeSource(input))
                .OutputToPipe(new StreamPipeSink(output), options => options
                    .ForceFormat(codec)
                    .WithAudioBitrate(AudioQuality.Low))
                .ProcessSynchronously();
            
            return output.ToArray();
        }
        catch (IOException ex)
        {
            return Array.Empty<byte>();
        }
    }

    private WaveStream GetSourceWaveStream(Stream input, AudioInfo srcParams)
    {
        return srcParams.AudioFormat switch
        {
            AudioFormat.Mp3     => new Mp3FileReader(input),
            AudioFormat.Ogg     => new VorbisWaveReader(input),
            _                   => new WaveFileReader(input)
        };
    }

    private MemoryStream GetTargetWaveStream(WaveStream input, AudioInfo srcParams)
    {
        input.Seek(0, SeekOrigin.Begin);
        var ms = new MemoryStream();
        
        Stream writerStream = srcParams.AudioFormat switch
        {
            AudioFormat.Mp3     => new LameMP3FileWriter(ms, input.WaveFormat, LAMEPreset.ABR_16),
            //AudioFormat.Wav     => new WavFileWriter(input),
            //AudioFormat.Aac     => new WavFileWriter(input),
            //AudioFormat.M4a     => new WavFileWriter(input),
            //AudioFormat.Unknown => new WavFileWriter(input),
            _ => new WaveFileWriter(ms, input.WaveFormat)
        };

        input.CopyTo(writerStream);

        writerStream.Dispose();
        return ms;
    }
}