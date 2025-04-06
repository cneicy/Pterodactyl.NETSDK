namespace Pterodactyl.NETSDK.Models.Application.Servers;

public class ServerResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }
    [JsonPropertyName("attributes")] public Server? Server { get; set; }
}