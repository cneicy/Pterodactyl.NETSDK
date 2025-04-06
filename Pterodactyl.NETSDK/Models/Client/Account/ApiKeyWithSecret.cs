namespace Pterodactyl.NETSDK.Models.Client.Account;

public class ApiKeyWithSecret : ApiKey
{
    [JsonPropertyName("token")] public string? SecretToken { get; set; }
}