namespace Pterodactyl.NETSDK.Request;

public class LocationCreateRequest
{
    [JsonPropertyName("short")] public string? ShortCode { get; set; }
    [JsonPropertyName("long")] public string? LongName { get; set; }
}