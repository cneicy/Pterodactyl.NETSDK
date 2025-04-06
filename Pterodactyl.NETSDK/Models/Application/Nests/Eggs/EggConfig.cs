using Pterodactyl.NETSDK.Models.Application.Servers;

namespace Pterodactyl.NETSDK.Models.Application.Nests.Eggs;

public class EggConfig
{
    [JsonPropertyName("files")] public Dictionary<string, FileConfig>? Files { get; set; }
    [JsonPropertyName("startup")] public StartupConfig? Startup { get; set; }
    [JsonPropertyName("stop")] public string? StopSign { get; set; }
    [JsonPropertyName("logs")] public List<LogConfig>? Logs { get; set; }
}