namespace Pterodactyl.NETSDK.Models.Common.Allocations;

public class AllocationResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }
    [JsonPropertyName("attributes")] public Allocation? Allocation { get; set; }
}