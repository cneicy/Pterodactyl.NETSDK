namespace Pterodactyl.NETSDK.Models.Application.Nodes;

public class NodeResponse
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("attributes")] public Node? Node { get; set; }
}