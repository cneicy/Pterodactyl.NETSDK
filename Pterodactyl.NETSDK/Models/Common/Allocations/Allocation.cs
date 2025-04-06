namespace Pterodactyl.NETSDK.Models.Common.Allocations;

public class Allocation
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("ip")] public string? Ip { get; set; }
    [JsonPropertyName("port")] public int Port { get; set; }
    [JsonPropertyName("notes")] public string? Notes { get; set; }
    [JsonPropertyName("is_default")] public bool IsDefault { get; set; }
}