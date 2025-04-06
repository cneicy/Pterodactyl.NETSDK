using System.Text.Json;
using Pterodactyl.NETSDK.Exceptions;

namespace Pterodactyl.NETSDK.Helper;

internal static class HttpHelper
{
    public static async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new PterodactylApiException(
                $"API请求失败，状态码: {response.StatusCode}" +
                response.StatusCode +
                content
            );

        try
        {
            return JsonSerializer.Deserialize<T>(content);
        }
        catch (JsonException ex)
        {
            throw new PterodactylApiException(
                $"JSON解析失败: {ex.Message}" +
                response.StatusCode +
                content +
                ex
            );
        }
    }

    public static async Task HandleResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new PterodactylApiException(
                $"API request failed with status code {response.StatusCode}: {errorContent}");
        }
    }
}