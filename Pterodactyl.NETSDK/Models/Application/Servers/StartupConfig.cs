namespace Pterodactyl.NETSDK.Models.Application.Servers;

public class StartupConfig
{
    [JsonPropertyName("done")] public string? CompletionExpression { get; set; }
    [JsonPropertyName("userInteraction")] public List<string>? UserInteractions { get; set; }
}