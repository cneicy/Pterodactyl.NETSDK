namespace Pterodactyl.NETSDK.Models.Client.Account;

public class AccountResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }
    [JsonPropertyName("attributes")] public Account? Account { get; set; }
}