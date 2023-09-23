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
        var srcParams = _analyzer.Analyze(input);

        //if (targetParams.AudioFormat == AudioFormat.Opus)
        //    return ProcessByStreamEncoder<OpusEncoder, OpusAudioEncoderConfiguration>(input, srcParams);
        //if (targetParams.AudioFormat == AudioFormat.M4a || targetParams.AudioFormat == AudioFormat.Aac)
        //    return ProcessByStreamEncoder<AACEncoder, AacAudioEncoderConfiguration>(input, srcParams);


        using var resultStream = new MemoryStream();
        using var srcStream = GetSourceWaveStream(input, srcParams);
        using var tgtStream = GetTargetWaveStream(srcStream, targetParams);
        var result = tgtStream.ToArray();
        return result;
    }

    //private static byte[] ProcessByStreamEncoder<TEnc, TConfig>(Stream input, AudioInfo srcParams)
    //where TEnc : IAudioEncoderBase, new()
    //where TConfig : AudioEncoderConfiguration, new()
    //{
    //    using var enc = new TEnc();

    //    InitStreamEncoder<TEnc, TConfig>(srcParams, enc);

    //    using var sr = new BinaryReader(input);

    //    var bytes = sr.ReadBytes((int)input.Length);
    //    enc.Transform(new MediaBuffer<byte>(bytes));

    //    return enc.OutputQueue?.LastOrDefault()?.Buffer?.Array;
    //}

    //private static void InitStreamEncoder<TEnc, TConfig>(AudioInfo srcParams, TEnc enc)
    //    where TEnc : IAudioEncoderBase, new()
    //    where TConfig : AudioEncoderConfiguration, new()
    //{
    //    enc.Init(new TConfig()
    //    {
    //        Channels = srcParams.Channels,
    //        Samplerate = srcParams.SampleRate,
    //        BitsPerSample = srcParams.BitsPerSample,
    //        Bitrate = srcParams.Bitrate
    //    });
    //}

    private WaveStream GetSourceWaveStream(Stream input, AudioInfo srcParams)
    {
        return srcParams.AudioFormat switch
        {
            AudioFormat.Mp3     => new Mp3FileReader(input),
            AudioFormat.Ogg     => new VorbisWaveReader(input),
            //AudioFormat.Wav     => new WaveFileReader(input),
            //AudioFormat.Aac     => new WaveFileReader(input),
            //AudioFormat.M4a     => new WaveFileReader(input),
            //AudioFormat.Unknown => new WaveFileReader(input),
            _                   => new WaveFileReader(input)
        };
    }

    private MemoryStream GetTargetWaveStream(WaveStream input, AudioInfo srcParams)
    {
        input.Seek(0, SeekOrigin.Begin);
        var ms = new MemoryStream();
        
        Stream writerStream = srcParams.AudioFormat switch
        {
            AudioFormat.Mp3     => new LameMP3FileWriter(ms, input.WaveFormat, LAMEPreset.MEDIUM_FAST),
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