namespace Pterodactyl.NETSDK.Models.Common;

public class PaginationMeta
{
    [JsonPropertyName("pagination")] public PaginationDetails? Pagination { get; set; }
}