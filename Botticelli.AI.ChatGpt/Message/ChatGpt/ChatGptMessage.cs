﻿using System.Text.Json.Serialization;

namespace Botticelli.AI.ChatGpt.Message.ChatGpt;

public class ChatGptMessage
{
    [JsonPropertyName("role")] public string Role { get; set; }

    [JsonPropertyName("content")] public string Content { get; set; }
}