namespace Pterodactyl.NETSDK.Models.Application.Nodes;

public class NodeConfiguration
{
    [JsonPropertyName("debug")] public bool Debug { get; set; }
    [JsonPropertyName("uuid")] public string? Uuid { get; set; }
    [JsonPropertyName("token_id")] public string? TokenId { get; set; }
    [JsonPropertyName("token")] public string? Token { get; set; }
    [JsonPropertyName("api")] public NodeApiConfig? Api { get; set; }
    [JsonPropertyName("system")] public NodeSystemConfig? System { get; set; }
    [JsonPropertyName("remote")] public string? Remote { get; set; }
}