using Botticelli.Talks;
using Botticelli.Talks.Extensions;
using Botticelli.Talks.OpenTts;
using Botticelli.Talks.Settings;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Botticelli.Talks.Tests.Extensions;

[TestFixture]
public class ServiceExtensionsTests
{
    [Test]
    public void AddOpenTtsTalks_Should_RegisterSpeakerAndSettings()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{nameof(TtsSettings)}:{nameof(TtsSettings.EngineConnection)}"] = "http://localhost:5500",
                [$"{nameof(TtsSettings)}:{nameof(TtsSettings.DefaultVoice)}"] = "voice",
                [$"{nameof(TtsSettings)}:{nameof(TtsSettings.Language)}"] = "en"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddOpenTtsTalks(config);

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<ISpeaker>().Should().BeOfType<OpenTtsSpeaker>();
        provider.GetRequiredService<IOptions<TtsSettings>>().Value.EngineConnection.Should().Be("http://localhost:5500");
    }
}
