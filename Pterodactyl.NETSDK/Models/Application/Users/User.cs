namespace Pterodactyl.NETSDK.Models.Application.Users;

public class User
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("external_id")] public string? ExternalId { get; set; }

    [JsonPropertyName("uuid")] public string? Uuid { get; set; }

    [JsonPropertyName("username")] public string? Username { get; set; }

    [JsonPropertyName("email")] public string? Email { get; set; }

    [JsonPropertyName("first_name")] public string? FirstName { get; set; }

    [JsonPropertyName("last_name")] public string? LastName { get; set; }

    [JsonPropertyName("language")] public string? Language { get; set; }

    [JsonPropertyName("root_admin")] public bool IsRootAdmin { get; set; }

    [JsonPropertyName("2fa")] public bool TwoFactorEnabled { get; set; }

    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
}