namespace Pterodactyl.NETSDK.Request;

public class UserCreateRequest
{
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("first_name")] public string? FirstName { get; set; }
    [JsonPropertyName("last_name")] public string? LastName { get; set; }
    [JsonPropertyName("password")] public string? Password { get; set; }
    [JsonPropertyName("root_admin")] public bool IsRootAdmin { get; set; }
}