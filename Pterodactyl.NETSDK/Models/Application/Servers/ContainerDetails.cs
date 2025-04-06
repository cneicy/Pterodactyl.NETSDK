namespace Pterodactyl.NETSDK.Models.Application.Servers;

public class ContainerDetails
{
    [JsonPropertyName("startup_command")] public string? StartupCommand { get; set; }

    [JsonPropertyName("image")] public string? Image { get; set; }

    [JsonPropertyName("installed")] public int Installed { get; set; }

    [JsonPropertyName("environment")] public Dictionary<string, object>? Environment { get; set; }
}