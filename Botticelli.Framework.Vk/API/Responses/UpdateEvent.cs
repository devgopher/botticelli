﻿using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

/// <summary>
///     Particular events
/// </summary>
public class UpdateEvent
{
    /// <summary>
    ///     Event type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     Id of an event
    /// </summary>
    [JsonPropertyName("event_id")]
    public string EventId { get; set; }

    /// <summary>
    ///     Id of a group
    /// </summary>
    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    /// <summary>
    ///     Inner object
    /// </summary>
    [JsonPropertyName("object")]
    public Dictionary<string, JsonNode> Object { get; set; }
}