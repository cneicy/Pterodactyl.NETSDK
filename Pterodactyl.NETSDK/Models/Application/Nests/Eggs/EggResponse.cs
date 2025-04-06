namespace Pterodactyl.NETSDK.Models.Application.Nests.Eggs;

public class EggResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("attributes")] public Egg? Egg { get; set; }
}