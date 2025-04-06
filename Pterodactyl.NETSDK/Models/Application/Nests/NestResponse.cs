namespace Pterodactyl.NETSDK.Models.Application.Nests;

public class NestResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("attributes")] public Nest? Nest { get; set; }
}