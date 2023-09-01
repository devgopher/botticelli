﻿using Botticelli.Framework.Vk.API.Objects;
using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Requests
{
    public class SendMessageRequest
    {
        public SendMessageRequest()
            => RandomId = new Random(DateTime.Now.Millisecond).Next();

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        [JsonPropertyName("random_id")]
        public int RandomId { get; }
        [JsonPropertyName("message")]
        public string Body { get; set; }
        [JsonPropertyName("reply_to")]
        public string ReplyTo { get; set; }
        [JsonPropertyName("lat")]
        public decimal? Lat { get; set; }
        [JsonPropertyName("long")]
        public decimal? Long { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("attachment")]
        public Attachment Attachment { get; set; }
        [JsonPropertyName("v")]
        public string ApiVersion => "5.131";
    }
}
