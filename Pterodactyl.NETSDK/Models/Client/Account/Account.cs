namespace Pterodactyl.NETSDK.Models.Client.Account;

public class Account
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("admin")] public bool Admin { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("first_name")] public string? FirstName { get; set; }
    [JsonPropertyName("last_name")] public string? LastName { get; set; }
    [JsonPropertyName("language")] public string? Language { get; set; }
}