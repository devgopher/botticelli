using Botticelli.Talks.Constants;
using Botticelli.Talks.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Talks.Tests.Settings;

[TestFixture]
public class TtsSettingsTests
{
    [Test]
    public void TtsSettings_Should_ExposeDefaultsAndAssignableProperties()
    {
        var settings = new TtsSettings
        {
            EngineType = "OpenTTS",
            EngineConnection = "http://localhost:5500",
            Language = "en",
            DefaultVoice = "female",
            Speed = 1.25,
            CompressionLevel = CompressionLevels.Medium
        };

        settings.EngineType.Should().Be("OpenTTS");
        settings.EngineConnection.Should().Be("http://localhost:5500");
        settings.Language.Should().Be("en");
        settings.DefaultVoice.Should().Be("female");
        settings.Speed.Should().Be(1.25);
        settings.CompressionLevel.Should().Be(CompressionLevels.Medium);
        new TtsSettings().Speed.Should().Be(1.0);
    }
}
