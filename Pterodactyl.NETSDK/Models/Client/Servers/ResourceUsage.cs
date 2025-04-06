namespace Pterodactyl.NETSDK.Models.Client.Servers;

public class ResourceUsage
{
    [JsonPropertyName("current_state")] public string? CurrentState { get; set; }
    [JsonPropertyName("is_suspended")] public bool IsSuspended { get; set; }
    [JsonPropertyName("resources")] public ResourceStats? Resources { get; set; }
}