namespace Pterodactyl.NETSDK.Models.Client.Account;

public class TwoFactorDetailsResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }
    [JsonPropertyName("attributes")] public TwoFactorDetails? TwoFactorDetails { get; set; }
}