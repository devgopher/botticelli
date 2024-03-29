﻿using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests;

public class Contact
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }
}