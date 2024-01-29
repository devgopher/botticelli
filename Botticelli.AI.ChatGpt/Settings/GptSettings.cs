using Botticelli.AI.Settings;

namespace Botticelli.AI.ChatGpt.Settings;

public class GptSettings : AiSettings
{
    public string ApiKey { get; set; }
    public string Model { get; set; }
    public double Temperature { get; set; }
}