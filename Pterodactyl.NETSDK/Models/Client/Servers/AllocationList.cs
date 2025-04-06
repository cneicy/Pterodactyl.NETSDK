using Pterodactyl.NETSDK.Models.Common.Allocations;

namespace Pterodactyl.NETSDK.Models.Client.Servers;

public class AllocationList
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("data")] public List<AllocationResponse>? Data { get; set; }
}