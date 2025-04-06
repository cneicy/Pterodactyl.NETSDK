namespace Pterodactyl.NETSDK.Models.Client.Account;

public class TwoFactorDetails
{
    [JsonPropertyName("image_url_data")] public string? ImageUrlData { get; set; }
}