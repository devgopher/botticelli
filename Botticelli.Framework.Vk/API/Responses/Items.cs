﻿using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class Items
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("items")]
    public Items Items1 { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("$ref")]
    public string Ref { get; set; }

    [JsonPropertyName("minimum")]
    public int Minimum { get; set; }

    [JsonPropertyName("entity")]
    public string Entity { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}