using FileSignatures;
using NAudio.Lame;
using NAudio.Vorbis;
using NAudio.Wave;
using StreamCoders;
using StreamCoders.Encoder;

namespace Botticelli.Audio;

public class UniversalConvertor : IConvertor
{
    private readonly IAnalyzer _analyzer;

    public UniversalConvertor(IAnalyzer analyzer) => _analyzer = analyzer;

    public byte[] Convert(Stream input, AudioInfo targetParams)
    {
        var srcParams = _analyzer.Analyze(input);

        if (targetParams.AudioFormat == AudioFormat.Opus)
        {
            using var enc = new OpusEncoder();

            enc.Init(new OpusAudioEncoderConfiguration()
            {
                BitrateMode = BitrateMode.Constant,
                Channels = srcParams.Channels,
                Samplerate = srcParams.SampleRate,
                BitsPerSample = srcParams.BitsPerSample,
                Bitrate = srcParams.Bitrate,
                Application = OpusApplication.Voip
            });

            using var sr = new BinaryReader(input);

            var bytes = sr.ReadBytes((int)input.Length);
            enc.Transform(new MediaBuffer<byte>(bytes));

            return enc.OutputQueue?.LastOrDefault()?.Buffer?.Array;
        }

        using var resultStream = new MemoryStream();
        using var srcStream = GetSourceWaveStream(input, srcParams);
        using var tgtStream = GetTargetWaveStream(srcStream, targetParams);

        using var outputSr = new BinaryReader(tgtStream);
        byte[] result = outputSr.ReadBytes((int) tgtStream.Length);
        return result;
    }

    private WaveStream GetSourceWaveStream(Stream input, AudioInfo srcParams)
    {
        WaveStream result = srcParams.AudioFormat switch
        {
            AudioFormat.Mp3     => new Mp3FileReader(input),
            AudioFormat.Ogg     => new VorbisWaveReader(input),
            //AudioFormat.Wav     => new WaveFileReader(input),
            //AudioFormat.Aac     => new WaveFileReader(input),
            //AudioFormat.M4a     => new WaveFileReader(input),
            //AudioFormat.Unknown => new WaveFileReader(input),
            _                   => new WaveFileReader(input)
        };

        return result;
    }

    private Stream GetTargetWaveStream(Stream input, AudioInfo srcParams)
    {
        input.Seek(0, SeekOrigin.Begin);

        MemoryStream ms = new MemoryStream();
        var waveFormat = CreateWaveFormat(srcParams);

        Stream writerStream = srcParams.AudioFormat switch
        {
            AudioFormat.Mp3     => new LameMP3FileWriter(ms, waveFormat , LAMEPreset.MEDIUM_FAST ),
            //AudioFormat.Wav     => new WavFileWriter(input),
            //AudioFormat.Aac     => new WavFileWriter(input),
            //AudioFormat.M4a     => new WavFileWriter(input),
            //AudioFormat.Unknown => new WavFileWriter(input),
            _ => default
        };

        input.CopyTo(writerStream);

        writerStream.Flush();

        return ms;
    }

    private static WaveFormat CreateWaveFormat(AudioInfo srcParams) => new(srcParams.SampleRate, srcParams.Channels);
}