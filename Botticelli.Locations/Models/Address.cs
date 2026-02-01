using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace Botticelli.Locations.Models;

/// <summary>
/// Represents a geographic address with many optional components.
/// </summary>
public class Address
{
    // Basic identifiers
    [JsonPropertyName("ObjectId")] public string ObjectId { get; set; }
    [JsonPropertyName("ObjectType")] public string ObjectType { get; set; }

    // Location details (all nullable because many fields may be missing)
    [JsonPropertyName("country")] public string? Country { get; set; }
    [JsonPropertyName("country_code")] public string? CountryCode { get; set; }
    [JsonPropertyName("county")] public string? County { get; set; }
    [JsonPropertyName("house_number")] public string? HouseNumber { get; set; }
    [JsonPropertyName("postcode")] public string? PostCode { get; set; }
    [JsonPropertyName("road")] public string? Road { get; set; }
    [JsonPropertyName("state")] public string? State { get; set; }
    [JsonPropertyName("town")] public string? Town { get; set; }
    [JsonPropertyName("pedestrian")] public string? Pedestrian { get; set; }
    [JsonPropertyName("neighborhood")] public string? Neighborhood { get; set; }
    [JsonPropertyName("hamlet")] public string? Hamlet { get; set; }
    [JsonPropertyName("suburb")] public string? Suburb { get; set; }
    [JsonPropertyName("village")] public string? Village { get; set; }
    [JsonPropertyName("city")] public string? City { get; set; }
    [JsonPropertyName("region")] public string? Region { get; set; }
    [JsonPropertyName("state_district")] public string? District { get; set; }

    // Geographic coordinates
    [JsonPropertyName("lat")] public double Latitude { get; set; }
    [JsonPropertyName("lon")] public double Longitude { get; set; }

    public string? DisplayName { get; set; }

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Formats the address according to the supplied culture.
    /// Supports a variety of locale‑specific patterns.
    /// </summary>
    protected virtual string ToString(CultureInfo culture)
    {
        var sb = new StringBuilder(4192);
        var locale = culture.Name.ToLowerInvariant();

        // -------------------- Russian (ru-RU) --------------------
        if (locale == "ru-ru")
        {
            sb.Append($"{Country}, {PostCode}, {Region}");
            AppendIfNotNull(sb, State);
            AppendIfNotNull(sb, County);
            AppendIfNotNull(sb, City);
            AppendIfNotNull(sb, Town);
            AppendIfNotNull(sb, Suburb);
            AppendIfNotNull(sb, District);
            AppendIfNotNull(sb, Village);
            AppendIfNotNull(sb, Hamlet);
            AppendIfNotNull(sb, Neighborhood);
            AppendIfNotNull(sb, Road);
            AppendIfNotNull(sb, HouseNumber);
        }
        // -------------------- English (US) --------------------
        else if (locale == "en-us")
        {
            sb.Append($"{Road}, {HouseNumber}, {City}, {State} {PostCode}");
            AppendIfNotNull(sb, County);
            AppendIfNotNull(sb, Country);
        }
        // -------------------- French (fr-FR) --------------------
        else if (locale == "fr-fr")
        {
            sb.Append($"{HouseNumber} {Road}, {PostCode} {City}");
        }
        // -------------------- German (de-DE) --------------------
        else if (locale == "de-de")
        {
            sb.Append($"{Road} {HouseNumber}, {PostCode} {City}");
        }
        // -------------------- English (GB) --------------------
        else if (locale == "en-gb")
        {
            sb.Append($"{Road}, {HouseNumber}, {City}, {PostCode}");
        }
        // -------------------- Italian (it-IT) --------------------
        else if (locale == "it-it")
        {
            sb.Append($"{HouseNumber} {Road}, {PostCode}, {City}, {Region}");
        }
        // -------------------- Chinese (zh-CMN) --------------------
        else if (locale == "zh-cmn")
        {
            sb.Append($"{State}, {City}, {District}, {Road}, {HouseNumber}");
        }
        // -------------------- Spanish (es-ES) --------------------
        else if (locale == "es-es")
        {
            // Example: Calle Nº, CP Ciudad, Provincia, País
            sb.Append($"{Road} {HouseNumber}, {PostCode} {City}");
            AppendIfNotNull(sb, Region);
            AppendIfNotNull(sb, Country);
        }
        // -------------------- Portuguese (pt-BR) --------------------
        else if (locale == "pt-br")
        {
            // Example: Rua, Nº, Bairro, CP Cidade - Estado, País
            sb.Append($"{Road}, {HouseNumber}");
            AppendIfNotNull(sb, Neighborhood);
            AppendIfNotNull(sb, Suburb);
            AppendIfNotNull(sb, PostCode);
            AppendIfNotNull(sb, City);
            AppendIfNotNull(sb, State);
            AppendIfNotNull(sb, Country);
        }
        // -------------------- Dutch (nl-NL) --------------------
        else if (locale == "nl-nl")
        {
            sb.Append($"{Road} {HouseNumber}, {PostCode} {City}");
            AppendIfNotNull(sb, Region);
            AppendIfNotNull(sb, Country);
        }
        // -------------------- Japanese (ja-JP) --------------------
        else if (locale == "ja-jp")
        {
            // Example: 〒PostCode City Ward Road HouseNumber
            sb.Append($"{PostCode} {City}");
            AppendIfNotNull(sb, District);
            AppendIfNotNull(sb, Road);
            AppendIfNotNull(sb, HouseNumber);
            AppendIfNotNull(sb, Country);
        }
        // -------------------- Default fallback --------------------
        else
        {
            sb.Append($"{Road}, {HouseNumber}");
            AppendIfNotNull(sb, District);
            AppendIfNotNull(sb, City);
            AppendIfNotNull(sb, Town);
            AppendIfNotNull(sb, Suburb);
            AppendIfNotNull(sb, Village);
            AppendIfNotNull(sb, Hamlet);
            AppendIfNotNull(sb, Neighborhood);
            AppendIfNotNull(sb, County);
            AppendIfNotNull(sb, State);
            AppendIfNotNull(sb, Country);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Helper that appends a comma‑separated value only when the value is not null/empty.
    /// </summary>
    private static void AppendIfNotNull(StringBuilder sb, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            sb.Append($", {value}");
    }
}
