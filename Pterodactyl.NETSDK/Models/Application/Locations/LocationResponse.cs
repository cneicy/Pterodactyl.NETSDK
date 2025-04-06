namespace Pterodactyl.NETSDK.Models.Application.Locations;

public class LocationResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("attributes")] public Location? Location { get; set; }
}