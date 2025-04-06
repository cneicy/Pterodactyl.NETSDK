namespace Pterodactyl.NETSDK.Models.Client.Servers;

public class WebSocketCredentials
{
    [JsonPropertyName("token")] public string? Token { get; set; }

    [JsonPropertyName("socket")] public string? SocketUrl { get; set; }
}