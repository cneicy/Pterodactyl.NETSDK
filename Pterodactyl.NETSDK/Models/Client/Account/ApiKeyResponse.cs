namespace Pterodactyl.NETSDK.Models.Client.Account;

public class ApiKeyResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("attributes")] public ApiKey? ApiKey { get; set; }
}