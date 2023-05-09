namespace Botticelli.AI.Settings;

public class ChatGptSettings : AiSettings
{
    public string ApiKey { get; set; }
    public string Model { get; set; }
    public double Temperature { get; set; }
}