namespace Pterodactyl.NETSDK.Models.Client.Servers;

public class WebSocketResponse
{
    [JsonPropertyName("data")] public WebSocketCredentials? Data { get; set; }
}