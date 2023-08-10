using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Requests
{
    public class SendMessageRequest
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        [JsonPropertyName("random_id")]
        public int RandomId { get; set; }
        [JsonPropertyName("message")]
        public string Body { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("v")]
        public const string ApiVersion = "5.131";
    }
}
