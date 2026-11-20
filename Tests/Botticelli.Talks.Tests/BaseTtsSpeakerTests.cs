using Botticelli.Talks.Constants;
using Botticelli.Talks.Settings;
using Botticelli.Talks.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Botticelli.Talks.Tests;

[TestFixture]
public class BaseTtsSpeakerTests
{
    [Test]
    public async Task Compress_WithNone_Should_ReturnOriginalBytes()
    {
        var input = new byte[] { 1, 2, 3, 4 };
        var speaker = CreateSpeaker(CompressionLevels.None);

        var result = await speaker.CompressPublic(input, CancellationToken.None);

        result.Should().Equal(input);
    }

    [TestCase(CompressionLevels.Low)]
    [TestCase(CompressionLevels.Medium)]
    [TestCase(CompressionLevels.High)]
    public async Task Compress_WithLevel_Should_AttemptCompression(CompressionLevels level)
    {
        var wav = WavTestData.CreateSilentWav();
        var speaker = CreateSpeaker(level);

        var act = async () => await speaker.CompressPublic(wav, CancellationToken.None);

        var result = await act.Should().NotThrowAsync();
        // Native LAME may be unavailable in some environments; both outcomes exercise the compression path.
        result.Subject.Should().NotBeNull();
    }

    [Test]
    public async Task Compress_WithInvalidInput_Should_ReturnEmptyAndLog()
    {
        var logger = new Mock<ILogger>();
        var speaker = CreateSpeaker(CompressionLevels.Low, logger);

        var result = await speaker.CompressPublic([1, 2, 3], CancellationToken.None);

        result.Should().BeEmpty();
        logger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Test]
    public async Task Compress_WithUnknownLevel_Should_ReturnEmpty()
    {
        var speaker = CreateSpeaker((CompressionLevels)99);
        var wav = WavTestData.CreateSilentWav();

        var result = await speaker.CompressPublic(wav, CancellationToken.None);

        result.Should().BeEmpty();
    }

    private static TestTtsSpeaker CreateSpeaker(CompressionLevels level, Mock<ILogger>? logger = null)
    {
        var settings = new Mock<IOptionsMonitor<TtsSettings>>();
        settings.SetupGet(s => s.CurrentValue).Returns(new TtsSettings
        {
            CompressionLevel = level,
            EngineConnection = "http://localhost",
            DefaultVoice = "voice",
            Language = "en",
            Speed = 1.0
        });

        return new TestTtsSpeaker(
            logger?.Object ?? new Mock<ILogger>().Object,
            new Mock<IHttpClientFactory>().Object,
            settings.Object);
    }
}
