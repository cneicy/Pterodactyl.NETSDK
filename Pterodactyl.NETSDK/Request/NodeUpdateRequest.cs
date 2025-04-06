namespace Pterodactyl.NETSDK.Request;

public class NodeUpdateRequest : NodeCreateRequest
{
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("behind_proxy")] public bool BehindProxy { get; set; }
    [JsonPropertyName("maintenance_mode")] public bool MaintenanceMode { get; set; }
}