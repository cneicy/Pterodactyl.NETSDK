namespace Pterodactyl.NETSDK.Request;

public class UserUpdateRequest : UserCreateRequest
{
    [JsonPropertyName("language")] public string? Language { get; set; }
}