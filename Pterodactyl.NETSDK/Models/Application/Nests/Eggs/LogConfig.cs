namespace Pterodactyl.NETSDK.Models.Application.Nests.Eggs;

public class LogConfig
{
    [JsonPropertyName("custom")] public bool IsCustom { get; set; }
    [JsonPropertyName("location")] public string? Location { get; set; }
}