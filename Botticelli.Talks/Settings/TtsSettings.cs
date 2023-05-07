using Botticelli.Talks.Constants;

namespace Botticelli.Talks.Settings;

public class TtsSettings
{
    public string? EngineType { get; set; }
    public string? EngineConnection { get; set; }
    public string? Language { get; set; }
    public string? DefaultVoice { get; set; }
    public double Speed { get; set; } = 1.0;
    public CompressionLevels CompressionLevel { get; set; }
}