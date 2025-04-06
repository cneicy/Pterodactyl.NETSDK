namespace Pterodactyl.NETSDK.Request;

public class DatabaseCreateRequest
{
    [JsonPropertyName("database")] public string? DatabaseName { get; set; }
    [JsonPropertyName("remote")] public string? RemoteAccess { get; set; }
    [JsonPropertyName("host")] public int HostId { get; set; }
}