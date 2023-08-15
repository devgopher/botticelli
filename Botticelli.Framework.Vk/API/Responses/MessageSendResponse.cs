using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses
{
    public class MessageSendResponse
     {
         [JsonPropertyName("response")]
         public int Response { get; set; }

         [JsonPropertyName("peer_id")]
         public int PeerId { get; set; }

         [JsonPropertyName("message_id")]
         public int MessageId { get; set; }

         [JsonPropertyName("conversation_message_id")]
         public int ConversationMessageId { get; set; }

         [JsonPropertyName("error")]
         public string Error { get; set; }
     }

}
