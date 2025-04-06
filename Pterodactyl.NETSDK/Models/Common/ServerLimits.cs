namespace Pterodactyl.NETSDK.Models.Common;

public class ServerLimits
{
    [JsonPropertyName("memory")] public int Memory { get; set; }
    [JsonPropertyName("swap")] public int Swap { get; set; }
    [JsonPropertyName("disk")] public int Disk { get; set; }
    [JsonPropertyName("io")] public int Io { get; set; }
    [JsonPropertyName("cpu")] public int Cpu { get; set; }
}