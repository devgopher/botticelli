using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class Document
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("owner_id")] public int OwnerId { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }

    [JsonPropertyName("ext")] public string Ext { get; set; }

    [JsonPropertyName("date")] public int Date { get; set; }

    [JsonPropertyName("type")] public int Type { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("is_unsafe")] public int IsUnsafe { get; set; }
}