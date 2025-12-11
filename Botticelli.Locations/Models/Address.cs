using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace Botticelli.Locations.Models;

public class Address
{
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

    [JsonPropertyName("lat")] public double Latitude { get; set; }

    [JsonPropertyName("lon")] public double Longitude { get; set; }

    public string? DisplayName { get; set; }

    public override string ToString()
    {
        return ToString(CultureInfo.InvariantCulture);
    }

    protected virtual string ToString(CultureInfo culture)
    {
        var result = new StringBuilder(4192);

        switch (culture.Name.ToLowerInvariant())
        {
            case "ru-ru":
                result.Append($"{Country}, {PostCode}, {Region}");
                if (State != null) result.Append($", {State}");
                if (County != null) result.Append($", {County}");
                if (City != null) result.Append($", {City}");
                if (Town != null) result.Append($", {Town}");
                if (Suburb != null) result.Append($", {Suburb}");
                if (District != null) result.Append($", {District}");
                if (Village != null) result.Append($", {Village}");
                if (Hamlet != null) result.Append($", {Hamlet}");
                if (Neighborhood != null) result.Append($", {Neighborhood}");
                if (Road != null) result.Append($", {Road}");
                if (HouseNumber != null) result.Append($", {HouseNumber}");
                break;
            default:
                result = result.Append($"{Road}, {HouseNumber}");

                if (District != null) result.Append($", {District}");
                if (City != null) result.Append($", {City}");
                if (Town != null) result.Append($", {Town}");
                if (Suburb != null) result.Append($", {Suburb}");
                if (Village != null) result.Append($", {Village}");
                if (Hamlet != null) result.Append($", {Hamlet}");
                if (Neighborhood != null) result.Append($", {Neighborhood}");
                if (County != null) result.Append($", {County}");
                if (State != null) result.Append($", {State}");
                if (Country != null) result.Append($", {Country}");
                break;
        }

        return result.ToString();
    }
}