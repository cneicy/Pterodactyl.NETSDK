namespace Pterodactyl.NETSDK.Models.Client.Servers;

public class ResourceStats
{
    [JsonPropertyName("memory_bytes")] public long MemoryBytes { get; set; }
    [JsonPropertyName("cpu_absolute")] public long CpuAbsolute { get; set; }
    [JsonPropertyName("disk_bytes")] public long DiskBytes { get; set; }
    [JsonPropertyName("network_rx_bytes")] public long NetworkRxBytes { get; set; }
    [JsonPropertyName("network_tx_bytes")] public long NetworkTxBytes { get; set; }
}