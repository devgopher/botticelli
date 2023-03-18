using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Responses.Broadcast;

public class FailedList
{
    [JsonPropertyName("receiver")]
    public string Receiver { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("status_message")]
    public string StatusMessage { get; set; }
}

public class SendBroadcastMessageResponse
{
    [JsonPropertyName("message_token")]
    public long MessageToken { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("status_message")]
    public string StatusMessage { get; set; }

    [JsonPropertyName("failed_list")]
    public List<FailedList> FailedList { get; set; }
}