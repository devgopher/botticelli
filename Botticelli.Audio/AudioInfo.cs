namespace Botticelli.Audio
{
    public class AudioInfo
    {
        public AudioFormat AudioFormat { get; set; }
        public int Bitrate => BitsPerSample * SampleRate;
        public int BitsPerSample { get; set; }
        public int SampleRate { get; set; }
        public int Channels { get; set; }
    }
}