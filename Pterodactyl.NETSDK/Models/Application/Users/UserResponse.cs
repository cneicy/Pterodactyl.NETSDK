namespace Pterodactyl.NETSDK.Models.Application.Users;

public class UserResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }
    [JsonPropertyName("attributes")] public User? User { get; set; }
}