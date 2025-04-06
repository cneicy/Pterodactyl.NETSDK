namespace Pterodactyl.NETSDK.Models.Application.Nodes;

public class NodeSystemConfig
{
    [JsonPropertyName("data")] public string? DataPath { get; set; }
    [JsonPropertyName("sftp")] public NodeSftpConfig? Sftp { get; set; }
}