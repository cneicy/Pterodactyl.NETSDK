using System.Text;
using System.Text.Json;
using System.Web;
using Pterodactyl.NETSDK.Helper;
using Pterodactyl.NETSDK.Models.Application.Locations;
using Pterodactyl.NETSDK.Models.Application.Nests;
using Pterodactyl.NETSDK.Models.Application.Nests.Eggs;
using Pterodactyl.NETSDK.Models.Application.Nodes;
using Pterodactyl.NETSDK.Models.Application.Servers;
using Pterodactyl.NETSDK.Models.Application.Users;
using Pterodactyl.NETSDK.Models.Common;
using Pterodactyl.NETSDK.Models.Common.Allocations;
using Pterodactyl.NETSDK.Request;

namespace Pterodactyl.NETSDK;

public class Application(string baseUrl, string apiKey) : PterodactylClient(baseUrl, apiKey)
{
    #region Application API

    #region Users

    public async Task<List<User?>> ListUsers()
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/users");
        var result = await HttpHelper.HandleResponse<PaginatedResult<UserResponse>>(response);
        return result?.Data.Select(r => r.User).ToList() ?? [];
    }

    public async Task<User?> GetUser(int userId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/users/{userId}");
        var result = await HttpHelper.HandleResponse<UserResponse>(response);
        return result?.User;
    }

    public async Task<User?> GetUserByExternalId(string externalId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/users/external/{externalId}");
        var result = await HttpHelper.HandleResponse<UserResponse>(response);
        return result?.User;
    }

    public async Task<User?> CreateUser(UserCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync($"{BaseUrl}/api/application/users", content);
        var result = await HttpHelper.HandleResponse<UserResponse>(response);
        return result?.User;
    }

    public async Task<User?> UpdateUser(int userId, Action<UserUpdateRequest> updateAction)
    {
        var currentUser = await GetUser(userId);

        var request = new UserUpdateRequest
        {
            Email = currentUser?.Email,
            Username = currentUser?.Username,
            FirstName = currentUser?.FirstName,
            LastName = currentUser?.LastName,
            Language = currentUser?.Language
        };

        updateAction(request);

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await HttpClient.PatchAsync($"{BaseUrl}/api/application/users/{userId}", content);
        var result = await HttpHelper.HandleResponse<UserResponse>(response);
        return result?.User;
    }

    public async Task DeleteUser(int userId)
    {
        var response = await HttpClient.DeleteAsync($"{BaseUrl}/api/application/users/{userId}");
        await HttpHelper.HandleResponse(response);
    }

    #endregion

    #region Nodes

    public async Task<List<Node>?> ListNodes()
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/nodes");
        var result = await HttpHelper.HandleResponse<PaginatedResult<NodeResponse>>(response);

        return result?.Data.Select(n => n.Node).ToList() ?? [];
    }

    public async Task<Node?> GetNode(int nodeId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/nodes/{nodeId}");
        var result = await HttpHelper.HandleResponse<NodeResponse>(response);
        return result?.Node;
    }

    public async Task<NodeConfiguration?> GetNodeConfiguration(int nodeId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/nodes/{nodeId}/configuration");
        return await HttpHelper.HandleResponse<NodeConfiguration>(response);
    }


    public async Task<Node?> CreateNode(NodeCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync($"{BaseUrl}/api/application/nodes", content);
        var result = await HttpHelper.HandleResponse<NodeResponse>(response);
        return result?.Node;
    }

    public async Task<Node?> UpdateNode(int nodeId, NodeUpdateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await HttpClient.PatchAsync($"{BaseUrl}/api/application/nodes/{nodeId}", content);
        var result = await HttpHelper.HandleResponse<NodeResponse>(response);
        return result?.Node;
    }

    public async Task DeleteNode(int nodeId)
    {
        var response = await HttpClient.DeleteAsync($"{BaseUrl}/api/application/nodes/{nodeId}");
        await HttpHelper.HandleResponse(response);
    }

    public async Task<List<Allocation>?> GetNodeAllocations(int nodeId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/nodes/{nodeId}/allocations");
        var result = await HttpHelper.HandleResponse<PaginatedResult<AllocationResponse>>(response);
        return result?.Data.Select(r => r.Allocation).ToList();
    }

    #endregion

    #region Servers

    public async Task<List<Server>?> ListServers()
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/servers");
        var result = await HttpHelper.HandleResponse<PaginatedResult<ServerResponse>>(response);

        return result?.Data.Select(r => r.Server).ToList() ?? [];
    }

    public async Task<Server?> GetServer(int serverId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/servers/{serverId}");
        var result = await HttpHelper.HandleResponse<ServerResponse>(response);
        return result?.Server;
    }

    public async Task<Server?> GetServerByExternalId(string externalId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/servers/external/{externalId}");
        var result = await HttpHelper.HandleResponse<ServerResponse>(response);
        return result?.Server;
    }

    public async Task<Server?> CreateServer(ServerCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync($"{BaseUrl}/api/application/servers", content);
        var result = await HttpHelper.HandleResponse<ServerResponse>(response);
        return result?.Server;
    }

    public async Task<Server?> UpdateServerDetails(int serverId,
        ServerDetailsUpdateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response =
            await HttpClient.PatchAsync($"{BaseUrl}/api/application/servers/{serverId}/details", content);
        var result = await HttpHelper.HandleResponse<ServerResponse>(response);
        return result?.Server;
    }

    public async Task SuspendServer(int serverId)
    {
        var response = await HttpClient.PostAsync($"{BaseUrl}/api/application/servers/{serverId}/suspend", null);
        await HttpHelper.HandleResponse(response);
    }

    public async Task UnSuspendServer(int serverId)
    {
        var response = await HttpClient.PostAsync($"{BaseUrl}/api/application/servers/{serverId}/unsuspend", null);
        await HttpHelper.HandleResponse(response);
    }

    public async Task ReinstallServer(int serverId)
    {
        var response = await HttpClient.PostAsync($"{BaseUrl}/api/application/servers/{serverId}/reinstall", null);
        await HttpHelper.HandleResponse(response);
    }

    public async Task DeleteServer(int serverId, bool force = false)
    {
        var url = $"{BaseUrl}/api/application/servers/{serverId}" + (force ? "/force" : "");
        var response = await HttpClient.DeleteAsync(url);
        await HttpHelper.HandleResponse(response);
    }

    #endregion

    #region Locations

    public async Task<List<Location>?> ListLocations()
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/locations");
        var result = await HttpHelper.HandleResponse<PaginatedResult<LocationResponse>>(response);
        return result?.Data.Select(r => r.Location).ToList() ?? [];
    }

    public async Task<Location?> CreateLocation(LocationCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync($"{BaseUrl}/api/application/locations", content);
        var result = await HttpHelper.HandleResponse<LocationResponse>(response);
        return result?.Location;
    }

    public async Task<Location?> UpdateLocation(int locationId, LocationUpdateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await HttpClient.PatchAsync($"{BaseUrl}/api/application/locations/{locationId}", content);
        var result = await HttpHelper.HandleResponse<LocationResponse>(response);
        return result?.Location;
    }

    public async Task DeleteLocation(int locationId)
    {
        var response = await HttpClient.DeleteAsync($"{BaseUrl}/api/application/locations/{locationId}");
        await HttpHelper.HandleResponse(response);
    }

    #endregion

    #region Databases 一般弃用 暂不进行适配

    public async Task<PaginatedResult<ServerDatabaseResponse>?> ListServerDatabases(int serverId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/servers/{serverId}/databases");
        return await HttpHelper.HandleResponse<PaginatedResult<ServerDatabaseResponse>>(response);
    }

    public async Task<ServerDatabaseResponse?> CreateServerDatabase(int serverId, DatabaseCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response =
            await HttpClient.PostAsync($"{BaseUrl}/api/application/servers/{serverId}/databases", content);
        return await HttpHelper.HandleResponse<ServerDatabaseResponse>(response);
    }

    public async Task ResetDatabasePassword(int serverId, int databaseId)
    {
        var response = await HttpClient.PostAsync(
            $"{BaseUrl}/api/application/servers/{serverId}/databases/{databaseId}/reset-password", null);
        await HttpHelper.HandleResponse(response);
    }

    public async Task DeleteServerDatabase(int serverId, int databaseId)
    {
        var response = await HttpClient.DeleteAsync(
            $"{BaseUrl}/api/application/servers/{serverId}/databases/{databaseId}");
        await HttpHelper.HandleResponse(response);
    }

    #endregion

    #region Nests

    public async Task<List<Nest>?> ListNests()
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/nests");
        var result = await HttpHelper.HandleResponse<PaginatedResult<NestResponse>>(response);
        return result?.Data.Select(n => n.Nest).ToList() ?? [];
    }

    public async Task<Nest?> GetNest(int nestId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/nests/{nestId}");
        var result = await HttpHelper.HandleResponse<NestResponse>(response);
        return result?.Nest;
    }

    public async Task<List<Egg?>> ListNestEggs(int nestId, bool includeNest = false, bool includeServers = false)
    {
        var includes = new List<string>();
        if (includeNest) includes.Add("nest");
        if (includeServers) includes.Add("servers");

        var query = includes.Any()
            ? $"?include={string.Join(",", includes)}"
            : string.Empty;

        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/nests/{nestId}/eggs{query}");
        var result = await HttpHelper.HandleResponse<PaginatedResult<EggResponse>>(response);

        return result?.Data.Select(r => r.Egg).ToList() ?? [];
    }

    public async Task<Egg?> GetEgg(int nestId, int eggId)
    {
        var response = await HttpClient.GetAsync($"{BaseUrl}/api/application/nests/{nestId}/eggs/{eggId}");
        var result = await HttpHelper.HandleResponse<EggResponse>(response);
        return result?.Egg;
    }

    #endregion

    #endregion
}