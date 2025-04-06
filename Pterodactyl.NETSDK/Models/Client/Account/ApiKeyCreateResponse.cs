namespace Pterodactyl.NETSDK.Models.Client.Account;

public class ApiKeyCreateResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("attributes")] public ApiKeyWithSecret? ApiKeyWithSecret { get; set; }
}