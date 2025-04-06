namespace Pterodactyl.NETSDK.Models.Client.Servers;

public class ServerRelationships
{
    [JsonPropertyName("allocations")] public AllocationList? Allocations { get; set; }
}