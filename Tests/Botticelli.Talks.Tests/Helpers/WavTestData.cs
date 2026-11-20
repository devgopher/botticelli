using NAudio.Wave;

namespace Botticelli.Talks.Tests.Helpers;

internal static class WavTestData
{
    public static byte[] CreateSilentWav(int sampleRate = 8000, int milliseconds = 50)
    {
        using var output = new MemoryStream();
        var format = new WaveFormat(sampleRate, 16, 1);
        using (var writer = new WaveFileWriter(new KeepOpenStream(output), format))
        {
            var bytes = sampleRate * 2 * milliseconds / 1000;
            writer.Write(new byte[bytes], 0, bytes);
        }

        return output.ToArray();
    }

    private sealed class KeepOpenStream(Stream inner) : Stream
    {
        public override bool CanRead => inner.CanRead;
        public override bool CanSeek => inner.CanSeek;
        public override bool CanWrite => inner.CanWrite;
        public override long Length => inner.Length;
        public override long Position
        {
            get => inner.Position;
            set => inner.Position = value;
        }

        public override void Flush() => inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => inner.Seek(offset, origin);
        public override void SetLength(long value) => inner.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => inner.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            // keep inner stream open for ToArray()
        }
    }
}
