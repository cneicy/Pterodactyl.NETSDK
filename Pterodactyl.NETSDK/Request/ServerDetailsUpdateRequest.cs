namespace Pterodactyl.NETSDK.Request;

public class ServerDetailsUpdateRequest
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("user")] public int UserId { get; set; }
    [JsonPropertyName("external_id")] public string? ExternalId { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
}