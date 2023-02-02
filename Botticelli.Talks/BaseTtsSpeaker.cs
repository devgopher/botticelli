using Botticelli.Talks.Constants;
using Botticelli.Talks.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NAudio.Lame;
using NAudio.Wave;

namespace Botticelli.Talks;

public abstract class BaseTtsSpeaker : ISpeaker
{
    protected readonly IHttpClientFactory HttpClientFactory;
    protected readonly ILogger Logger;
    protected readonly IOptionsMonitor<TtsSettings> Settings;

    protected BaseTtsSpeaker(ILogger logger,
                             IHttpClientFactory httpClientFactory,
                             IOptionsMonitor<TtsSettings> settings)
    {
        Logger = logger;
        HttpClientFactory = httpClientFactory;
        Settings = settings;
    }

    public abstract Task<byte[]> Speak(string markedText,
                                       string voice,
                                       string lang,
                                       double speed,
                                       CancellationToken token);

    public abstract Task<byte[]> Speak(string markedText,
                              string voice,
                              string lang,
                              CancellationToken token);

    public abstract Task<byte[]> Speak(string markedText, CancellationToken token);

    protected async Task<byte[]> Compress(byte[] input, CancellationToken token)
    {
        try
        {
            if (Settings.CurrentValue.CompressionLevel == CompressionLevels.None) return input;

            var preset = Settings.CurrentValue.CompressionLevel switch
            {
                CompressionLevels.Low => LAMEPreset.EXTREME_FAST,
                CompressionLevels.Medium => LAMEPreset.MEDIUM_FAST,
                CompressionLevels.High => LAMEPreset.ABR_16,
                _ => throw new ArgumentOutOfRangeException()
            };

            using var resultStream = new MemoryStream();
            using var bufferStream = new MemoryStream(input);
            using var wavReader = new WaveFileReader(bufferStream);

            using (var mp3Writer = new LameMP3FileWriter(resultStream, wavReader.WaveFormat, preset))
            {
                wavReader.CopyTo(mp3Writer);

                return resultStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error while compressing: {ex.Message}", ex);
        }

        return Array.Empty<byte>();
    }
}