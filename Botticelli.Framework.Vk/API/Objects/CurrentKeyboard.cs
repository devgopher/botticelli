﻿using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class CurrentKeyboard
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}