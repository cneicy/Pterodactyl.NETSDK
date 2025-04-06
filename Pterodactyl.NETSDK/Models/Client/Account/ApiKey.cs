namespace Pterodactyl.NETSDK.Models.Client.Account;

public class ApiKey
{
    [JsonPropertyName("identifier")] public string? Identifier { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("allowed_ips")] public List<string>? AllowedIps { get; set; }

    [JsonPropertyName("last_used_at")] public DateTime? LastUsedAt { get; set; }

    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
}