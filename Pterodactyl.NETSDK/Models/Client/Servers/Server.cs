using Pterodactyl.NETSDK.Models.Common;

namespace Pterodactyl.NETSDK.Models.Client.Servers;

public class Server
{
    [JsonPropertyName("server_owner")] public bool IsServerOwner { get; set; }

    [JsonPropertyName("identifier")] public string? Identifier { get; set; }

    [JsonPropertyName("uuid")] public string? Uuid { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("node")] public string? NodeName { get; set; }

    [JsonPropertyName("sftp_details")] public SftpDetails? SftpDetails { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("limits")] public ServerLimits? Limits { get; set; }

    [JsonPropertyName("feature_limits")] public ServerFeatureLimits? FeatureLimits { get; set; }

    [JsonPropertyName("is_suspended")] public bool IsSuspended { get; set; }

    [JsonPropertyName("is_installing")] public bool IsInstalling { get; set; }

    [JsonPropertyName("relationships")] public ServerRelationships? Relationships { get; set; }
}