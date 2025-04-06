namespace Pterodactyl.NETSDK.Models.Application.Servers;

public class ServerDatabaseResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("attributes")] public ServerDatabase? ServerDatabase { get; set; }
}