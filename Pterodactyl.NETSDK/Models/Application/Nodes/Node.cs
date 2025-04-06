namespace Pterodactyl.NETSDK.Models.Application.Nodes;

public class Node
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("uuid")] public string? Uuid { get; set; }
    [JsonPropertyName("public")] public bool Public { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("location_id")] public int LocationId { get; set; }
    [JsonPropertyName("fqdn")] public string? Fqdn { get; set; }
    [JsonPropertyName("scheme")] public string? Scheme { get; set; }
    [JsonPropertyName("behind_proxy")] public bool BehindProxy { get; set; }
    [JsonPropertyName("maintenance_mode")] public bool MaintenanceMode { get; set; }
    [JsonPropertyName("memory")] public int Memory { get; set; }

    [JsonPropertyName("memory_overallocate")]
    public int MemoryOverallocate { get; set; }

    [JsonPropertyName("disk")] public int Disk { get; set; }

    [JsonPropertyName("disk_overallocate")]
    public int DiskOverallocate { get; set; }

    [JsonPropertyName("upload_size")] public int UploadSize { get; set; }
    [JsonPropertyName("daemon_listen")] public int DaemonListen { get; set; }
    [JsonPropertyName("daemon_sftp")] public int DaemonSftp { get; set; }
    [JsonPropertyName("daemon_base")] public string? DaemonBase { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
}