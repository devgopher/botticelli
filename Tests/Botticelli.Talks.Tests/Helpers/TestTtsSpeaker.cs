using Botticelli.Talks.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.Talks.Tests.Helpers;

internal sealed class TestTtsSpeaker : BaseTtsSpeaker
{
    public TestTtsSpeaker(ILogger logger,
                          IHttpClientFactory httpClientFactory,
                          IOptionsMonitor<TtsSettings> settings)
        : base(logger, httpClientFactory, settings)
    {
    }

    public Task<byte[]> CompressPublic(byte[] input, CancellationToken token)
        => Compress(input, token);

    public override Task<byte[]> Speak(string markedText, string voice, string lang, CancellationToken token)
        => Task.FromResult(Array.Empty<byte>());

    public override Task<byte[]> Speak(string markedText, CancellationToken token)
        => Task.FromResult(Array.Empty<byte>());

    public override Task<byte[]> Speak(string markedText, string voice, string lang, double speed, CancellationToken token)
        => Task.FromResult(Array.Empty<byte>());
}
