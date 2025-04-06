using Pterodactyl.NETSDK.Models.Common;
using Pterodactyl.NETSDK.Models.Common.Allocations;

namespace Pterodactyl.NETSDK.Request;

public class ServerCreateRequest
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("user")] public int UserId { get; set; }
    [JsonPropertyName("egg")] public int EggId { get; set; }
    [JsonPropertyName("node")] public int NodeId { get; set; }

    [JsonPropertyName("allocation")] public AllocationSettings Allocation { get; set; } = new();

    [JsonPropertyName("docker_image")] public string? DockerImage { get; set; }
    [JsonPropertyName("startup")] public string? StartupCommand { get; set; }
    [JsonPropertyName("environment")] public Dictionary<string, string>? Environment { get; set; }
    [JsonPropertyName("limits")] public ServerLimits? Limits { get; set; }
    [JsonPropertyName("feature_limits")] public ServerFeatureLimits? FeatureLimits { get; set; }
}