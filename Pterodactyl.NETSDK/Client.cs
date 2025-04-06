using System.Text;
using System.Text.Json;
using Pterodactyl.NETSDK.Helper;
using Pterodactyl.NETSDK.Models.Client.Account;
using Pterodactyl.NETSDK.Models.Client.Servers;
using Pterodactyl.NETSDK.Models.Common;

namespace Pterodactyl.NETSDK;

public class Client(string baseUrl, string apiKey) : PterodactylClient(baseUrl, apiKey)
{
    public async Task<List<Server?>> ListServers()
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/client");
        var result = await HttpHelper.HandleResponse<PaginatedResult<ServerResponse>>(response);
        return result?.Data.Select(r => r.Server).ToList() ?? [];
    }

    public async Task<Account?> GetAccountDetails()
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/client/account");
        var result = await HttpHelper.HandleResponse<AccountResponse>(response);
        return result?.Account;
    }

    public async Task<TwoFactorDetails?> GetTwoFactorDetails()
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/client/account/two-factor");
        var result = await HttpHelper.HandleResponse<TwoFactorDetailsResponse>(response);
        return result?.TwoFactorDetails;
    }

    public async Task<TwoFactorEnableResponse?> EnableTwoFactor(string code)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { code }), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync($"{BaseUrl}/api/client/account/two-factor", content);
        return await HttpHelper.HandleResponse<TwoFactorEnableResponse>(response);
    }

    public async Task DisableTwoFactor(string password)
    {
        var requestBody = new { password };
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/api/client/account/two-factor")
        {
            Content = content
        };

        var response = await HttpClient.SendAsync(request);

        await HttpHelper.HandleResponse(response);
    }

    public async Task UpdateEmail(string email, string password)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { email, password }), Encoding.UTF8,
            "application/json");
        var response = await HttpClient.PutAsync($"{BaseUrl}/api/client/account/email", content);
        await HttpHelper.HandleResponse(response);
    }

    public async Task UpdatePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            current_password = currentPassword,
            password = newPassword,
            password_confirmation = confirmPassword
        }), Encoding.UTF8, "application/json");

        var response = await HttpClient.PutAsync($"{BaseUrl}/api/client/account/password", content);
        await HttpHelper.HandleResponse(response);
    }

    public async Task<List<ApiKey>?> ListApiKeys()
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/client/account/api-keys");
        var result = await HttpHelper.HandleResponse<PaginatedResult<ApiKeyResponse>>(response);
        return result?.Data.Select(r => r.ApiKey).ToList();
    }

    public async Task<ApiKeyWithSecret?> CreateApiKey(string description, List<string>? allowedIps = null)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            description,
            allowed_ips = allowedIps ?? []
        }), Encoding.UTF8, "application/json");

        var response = await HttpClient.PostAsync($"{BaseUrl}/api/client/account/api-keys", content);
        var result = await HttpHelper.HandleResponse<ApiKeyCreateResponse>(response);
        return result?.ApiKeyWithSecret;
    }

    public async Task DeleteApiKey(string identifier)
    {
        var response = await HttpClient.DeleteAsync($"{BaseUrl}/api/client/account/api-keys/{identifier}");
        await HttpHelper.HandleResponse(response);
    }

    public async Task<Server?> GetServerDetails(string serverId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/client/servers/{serverId}");
        var result = await HttpHelper.HandleResponse<ServerResponse>(response);
        return result?.Server;
    }

    public async Task SendCommand(string serverId, string command)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { command }), Encoding.UTF8,
            "application/json");
        var response = await HttpClient.PostAsync($"{BaseUrl}/api/client/servers/{serverId}/command", content);
        await HttpHelper.HandleResponse(response);
    }

    public async Task<WebSocketCredentials?> GetWebSocketCredentials(string serverId)
    {
        var response = await HttpClient.GetAsync(
            $"{BaseUrl}/api/client/servers/{serverId}/websocket"
        );
        return (await HttpHelper.HandleResponse<WebSocketResponse>(response))?.Data;
    }
}