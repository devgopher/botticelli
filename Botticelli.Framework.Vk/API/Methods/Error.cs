﻿// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Methods;

public class Error
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}