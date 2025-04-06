using Pterodactyl.NETSDK.Models.Common;

namespace Pterodactyl.NETSDK.Models.Application.Servers;

public class Server
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("identifier")] public string? Identifier { get; set; }
    [JsonPropertyName("external_id")] public string? ExternalId { get; set; }
    [JsonPropertyName("uuid")] public string? Uuid { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("suspended")] public bool IsSuspended { get; set; }
    [JsonPropertyName("limits")] public ServerLimits? Limits { get; set; }
    [JsonPropertyName("feature_limits")] public ServerFeatureLimits? FeatureLimits { get; set; }
    [JsonPropertyName("user")] public int User { get; set; }
    [JsonPropertyName("node")] public int Node { get; set; }
    [JsonPropertyName("allocation")] public int Allocation { get; set; }
    [JsonPropertyName("nest")] public int Nest { get; set; }
    [JsonPropertyName("egg")] public int Egg { get; set; }
    [JsonPropertyName("container")] public ContainerDetails? Container { get; set; }
    [JsonPropertyName("updated_at")] public DateTime? UpdateAt { get; set; }
    [JsonPropertyName("created_at")] public DateTime? CreatedAt { get; set; }
}