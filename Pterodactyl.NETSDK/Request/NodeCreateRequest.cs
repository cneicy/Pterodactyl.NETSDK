namespace Pterodactyl.NETSDK.Request;

public class NodeCreateRequest
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("location_id")] public int LocationId { get; set; }
    [JsonPropertyName("fqdn")] public string? Fqdn { get; set; }
    [JsonPropertyName("scheme")] public string? Scheme { get; set; }
    [JsonPropertyName("memory")] public int Memory { get; set; }

    [JsonPropertyName("memory_overallocate")]
    public int MemoryOverallocation { get; set; }

    [JsonPropertyName("disk")] public int Disk { get; set; }

    [JsonPropertyName("disk_overallocate")]
    public int DiskOverallocation { get; set; }

    [JsonPropertyName("upload_size")] public int UploadSize { get; set; }
    [JsonPropertyName("daemon_sftp")] public int SftpPort { get; set; }
    [JsonPropertyName("daemon_listen")] public int DaemonPort { get; set; }
}