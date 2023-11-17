using Newtonsoft.Json;

namespace Botticelli.AI.Message.GptJ;

public class GptJOutputMessage
{
    [JsonProperty("completion")] public string Completion { get; set; }
}