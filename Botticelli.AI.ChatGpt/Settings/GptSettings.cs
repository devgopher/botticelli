using Botticelli.AI.Settings;

namespace Botticelli.AI.ChatGpt.Settings;

public class GptSettings : AiSettings
{
    public string Model { get; set; }
    public double Temperature { get; set; }
}