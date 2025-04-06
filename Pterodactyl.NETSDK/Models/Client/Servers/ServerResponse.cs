namespace Pterodactyl.NETSDK.Models.Client.Servers;

public class ServerResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("attributes")] public Server? Server { get; set; }
}