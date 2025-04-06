namespace Pterodactyl.NETSDK.Models.Common;

public class PaginatedResult<T>
{
    [JsonPropertyName("object")] public string? ObjectType { get; set; }

    [JsonPropertyName("data")] public List<T>? Data { get; set; }

    [JsonPropertyName("meta")] public PaginationMeta? Meta { get; set; }
}