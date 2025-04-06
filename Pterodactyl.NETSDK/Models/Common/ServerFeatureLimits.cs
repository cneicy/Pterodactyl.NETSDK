namespace Pterodactyl.NETSDK.Models.Common;

public class ServerFeatureLimits
{
    [JsonPropertyName("databases")] public int Databases { get; set; }
    [JsonPropertyName("allocations")] public int Allocations { get; set; }
    [JsonPropertyName("backups")] public int Backups { get; set; }
}