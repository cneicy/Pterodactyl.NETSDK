namespace Pterodactyl.NETSDK.Models.Client.Servers;

public class SftpDetails
{
    [JsonPropertyName("ip")] public string? Ip { get; set; }

    [JsonPropertyName("port")] public int Port { get; set; }
}