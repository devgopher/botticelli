using Botticelli.Talks.Constants;
using Botticelli.Talks.OpenTts;
using Botticelli.Talks.Settings;
using Botticelli.Talks.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Botticelli.Talks.Tests.OpenTts;

[TestFixture]
public class OpenTtsSpeakerTests
{
    [Test]
    public async Task Speak_WithAllArgs_Should_ReturnCompressedAudio()
    {
        var wav = WavTestData.CreateSilentWav();
        var speaker = CreateSpeaker(wav, System.Net.HttpStatusCode.OK, CompressionLevels.None);

        var result = await speaker.Speak("hello", "alloy", "en", 1.1, CancellationToken.None);

        result.Should().Equal(wav);
    }

    [Test]
    public async Task Speak_VoiceLangOverload_Should_DelegateToSpeedOverload()
    {
        var wav = WavTestData.CreateSilentWav();
        var speaker = CreateSpeaker(wav, System.Net.HttpStatusCode.OK, CompressionLevels.None);

        var result = await speaker.Speak("hello", "alloy", "en", CancellationToken.None);

        result.Should().Equal(wav);
    }

    [Test]
    public async Task Speak_MarkedTextOnly_Should_UseDefaultSettings()
    {
        var wav = WavTestData.CreateSilentWav();
        var speaker = CreateSpeaker(wav, System.Net.HttpStatusCode.OK, CompressionLevels.None);

        var result = await speaker.Speak("hello", CancellationToken.None);

        result.Should().Equal(wav);
    }

    [Test]
    public async Task Speak_WhenHttpFails_Should_ReturnEmptyAndLog()
    {
        var logger = new Mock<ILogger<OpenTtsSpeaker>>();
        var speaker = CreateSpeaker([], System.Net.HttpStatusCode.BadRequest, CompressionLevels.None, logger);

        var result = await speaker.Speak("fail", "v", "en", 1.0, CancellationToken.None);

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

    private static OpenTtsSpeaker CreateSpeaker(
        byte[] responseBody,
        System.Net.HttpStatusCode status,
        CompressionLevels compression,
        Mock<ILogger<OpenTtsSpeaker>>? logger = null)
    {
        var handler = new MockHttpMessageHandler();
        handler.When("*").Respond(status, "audio/wav", new MemoryStream(responseBody));

        var settings = new Mock<IOptionsMonitor<TtsSettings>>();
        settings.SetupGet(s => s.CurrentValue).Returns(new TtsSettings
        {
            EngineConnection = "http://localhost:5500",
            DefaultVoice = "default-voice",
            Language = "en",
            Speed = 1.0,
            CompressionLevel = compression
        });

        return new OpenTtsSpeaker(
            new TestHttpClientFactory(handler),
            logger?.Object ?? new Mock<ILogger<OpenTtsSpeaker>>().Object,
            settings.Object);
    }

    private sealed class TestHttpClientFactory(HttpMessageHandler handler) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new(handler, disposeHandler: false);
    }
}
