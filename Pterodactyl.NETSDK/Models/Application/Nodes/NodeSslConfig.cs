namespace Pterodactyl.NETSDK.Models.Application.Nodes;

public class NodeSslConfig
{
    [JsonPropertyName("enabled")] public bool Enabled { get; set; }
    [JsonPropertyName("cert")] public string? Certificate { get; set; }
    [JsonPropertyName("key")] public string? Key { get; set; }
}