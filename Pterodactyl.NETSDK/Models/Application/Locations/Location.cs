namespace Pterodactyl.NETSDK.Models.Application.Locations;

public class Location
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("short")] public string? ShortCode { get; set; }
    [JsonPropertyName("long")] public string? LongName { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
}