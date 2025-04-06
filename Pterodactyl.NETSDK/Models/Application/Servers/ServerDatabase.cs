namespace Pterodactyl.NETSDK.Models.Application.Servers;

public class ServerDatabase
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("server")] public int ServerId { get; set; }
    [JsonPropertyName("host")] public int HostId { get; set; }
    [JsonPropertyName("database")] public string? DatabaseName { get; set; }
    [JsonPropertyName("username")] public string? Username { get; set; }
    [JsonPropertyName("remote")] public string? RemoteAccess { get; set; }
    [JsonPropertyName("max_connections")] public int MaxConnections { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
}