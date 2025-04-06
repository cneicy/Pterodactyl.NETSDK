namespace Pterodactyl.NETSDK.Models.Application.Nodes;

public class NodeApiConfig
{
    [JsonPropertyName("host")] public string? Host { get; set; }
    [JsonPropertyName("port")] public int Port { get; set; }
    [JsonPropertyName("ssl")] public NodeSslConfig? Ssl { get; set; }
}