namespace Pterodactyl.NETSDK.Models.Application.Nests.Eggs;

public class Egg
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("uuid")] public string? Uuid { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("docker_image")] public string? DockerImage { get; set; }
    [JsonPropertyName("config")] public EggConfig? Config { get; set; }
    [JsonPropertyName("startup")] public string? StartupCommand { get; set; }
    [JsonPropertyName("script")] public EggScript? Script { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
}