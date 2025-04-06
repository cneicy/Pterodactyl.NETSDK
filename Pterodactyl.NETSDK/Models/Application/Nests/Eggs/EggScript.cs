namespace Pterodactyl.NETSDK.Models.Application.Nests.Eggs;

public class EggScript
{
    [JsonPropertyName("privileged")] public bool Privileged { get; set; }
    [JsonPropertyName("install")] public string? InstallScript { get; set; }
    [JsonPropertyName("entry")] public string? EntryPoint { get; set; }
    [JsonPropertyName("container")] public string? ContainerImage { get; set; }
}