namespace Pterodactyl.NETSDK.Models.Application.Nests.Eggs;

public class FileConfig
{
    [JsonPropertyName("parser")] public string? Parser { get; set; }
    [JsonPropertyName("find")] public Dictionary<string, object>? FindReplace { get; set; }
}