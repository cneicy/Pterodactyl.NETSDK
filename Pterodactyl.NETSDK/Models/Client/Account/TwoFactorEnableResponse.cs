namespace Pterodactyl.NETSDK.Models.Client.Account;

public class TwoFactorEnableResponse
{
    [JsonPropertyName("tokens")] public List<string>? Tokens { get; set; }
    [JsonPropertyName("allowed_ips")] public List<string>? AllowedIps { get; set; }
    [JsonPropertyName("last_used_at")] public DateTime? LastUsedAt { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
}