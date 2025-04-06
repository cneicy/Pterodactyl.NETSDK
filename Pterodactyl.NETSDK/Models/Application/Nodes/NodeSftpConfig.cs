namespace Pterodactyl.NETSDK.Models.Application.Nodes;

public class NodeSftpConfig
{
    [JsonPropertyName("bind_port")] public int Port { get; set; }
}