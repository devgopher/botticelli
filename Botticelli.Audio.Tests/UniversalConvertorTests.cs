using NUnit.Framework;

namespace Botticelli.Audio.Tests
{
    [TestFixture()]
    public class UniversalConvertorTests
    {
        private readonly IConvertor _convertor;
        private readonly IAnalyzer _analyzer;

        public UniversalConvertorTests()
        {
            _analyzer = new InputAnalyzer();
            _convertor = new UniversalConvertor(_analyzer);
        }

        [Test()]
        public void ConvertMp3ToMp3Test()
        {
            if (!File.Exists("voice.mp3"))
                Assert.Fail("no voice.mp3!");
            using var stream = File.OpenRead("voice.mp3");
            var outcome = _convertor.Convert(stream, new AudioInfo()
            {
                AudioFormat = AudioFormat.Mp3,
                Channels = 2,
                SampleRate = 8000,
                BitsPerSample = 16
            });

            Assert.NotNull(outcome);
            Assert.IsNotEmpty(outcome);
        }
    }
}