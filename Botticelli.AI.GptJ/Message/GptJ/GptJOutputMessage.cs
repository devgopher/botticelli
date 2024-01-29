using Newtonsoft.Json;

namespace Botticelli.AI.GptJ.Message.GptJ;

public class GptJOutputMessage
{
    [JsonProperty("completion")] public string Completion { get; set; }
}