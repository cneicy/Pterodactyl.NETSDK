namespace Pterodactyl.NETSDK.Models.Common.Allocations;

public class AllocationSettings
{
    [JsonPropertyName("default")] public int DefaultAllocation { get; set; }

    [JsonPropertyName("additional")] public List<int> AdditionalAllocations { get; set; } = new();
}