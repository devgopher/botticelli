using Botticelli.Audio.Exceptions;
using FFMpegCore;
using FFMpegCore.Pipes;
using Microsoft.Extensions.Logging;
using NAudio.Lame;
using NAudio.Vorbis;
using NAudio.Wave;

namespace Botticelli.Audio.Convertors;

public class UniversalQualityConvertor(IAnalyzer analyzer, ILogger<UniversalQualityConvertor> logger)
    : IConvertor
{
    public byte[] Convert(Stream input, AudioInfo tgtParams)
    {
        try
        {
            if (tgtParams.AudioFormat is AudioFormat.M4a or AudioFormat.Aac or AudioFormat.Opus or AudioFormat.Ogg)
                return ProcessByStreamEncoder(input, tgtParams);

            var srcParams = analyzer.Analyze(input);

            using var resultStream = new MemoryStream();
            using var srcStream = GetSourceWaveStream(input, srcParams);
            using var tgtStream = GetTargetWaveStream(srcStream, tgtParams);

            return tgtStream.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{ConvertName} => ({TgtParamsAudioFormat}, {TgtParamsBitrate})!",
                nameof(Convert), tgtParams.AudioFormat, tgtParams.Bitrate);

            throw new AudioConvertorException($"Audio conversion error: {ex.Message}", ex);
        }
    }

    public byte[] Convert(byte[] input, AudioInfo tgtParams)
    {
        using var ms = new MemoryStream(input);

        return Convert(ms, tgtParams);
    }

    private byte[] ProcessByStreamEncoder(Stream input, AudioInfo tgtParams)
    {
        try
        {
            string codec;

            switch (tgtParams.AudioFormat)
            {
                case AudioFormat.Mp3:
                    codec = "mp3";

                    break;
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
                case AudioFormat.Wav:
                case AudioFormat.Unknown:
                default:
                    codec = "wav";
                    break;
            }

            using var output = new MemoryStream();
            FFMpegArguments
                .FromPipeInput(new StreamPipeSource(input))
                .OutputToPipe(new StreamPipeSink(output),
                    options => options
                        .ForceFormat(codec)
                        .WithAudioBitrate(tgtParams.Bitrate))
                .ProcessSynchronously(ffMpegOptions: new FFOptions
                {
                    BinaryFolder = "ffmpeg"
                });

            return output.ToArray();
        }
        catch (IOException ex)
        {
            logger.LogError(ex, "{ConvertName} => ({TgtParamsAudioFormat}, {TgtParamsBitrate}) error", nameof(Convert),
                tgtParams.AudioFormat, tgtParams.Bitrate);

            return [];
        }
    }

    private static WaveStream GetSourceWaveStream(Stream input, AudioInfo srcParams)
    {
        return srcParams.AudioFormat switch
        {
            AudioFormat.Mp3 => new Mp3FileReader(input),
            AudioFormat.Ogg => new VorbisWaveReader(input),
            _ => new WaveFileReader(input)
        };
    }

    private static MemoryStream GetTargetWaveStream(WaveStream input, AudioInfo tgtParams)
    {
        var ms = new MemoryStream();

        Stream writerStream = tgtParams.AudioFormat switch
        {
            AudioFormat.Mp3 => new LameMP3FileWriter(ms, input.WaveFormat, GetLamePreset(tgtParams)),
            _ => new WaveFileWriter(ms, input.WaveFormat)
        };

        input.CopyTo(writerStream);

        writerStream.Dispose();

        return ms;
    }

    private static LAMEPreset GetLamePreset(AudioInfo tgtParams) =>
        tgtParams.Bitrate switch
        {
            > 0 and <= 8 => LAMEPreset.ABR_8,
            > 8 and <= 16 => LAMEPreset.ABR_16,
            > 16 and <= 32 => LAMEPreset.ABR_32,
            > 32 and <= 48 => LAMEPreset.ABR_48,
            > 48 and <= 64 => LAMEPreset.ABR_64,
            > 64 and <= 128 => LAMEPreset.ABR_128,
            > 128 and <= 256 => LAMEPreset.ABR_256,
            _ => tgtParams.Bitrate is > 256 ? LAMEPreset.ABR_320 : LAMEPreset.ABR_64
        };
}