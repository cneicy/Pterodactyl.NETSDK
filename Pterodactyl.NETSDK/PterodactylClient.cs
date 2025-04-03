using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberHidesStaticFromOuterClass
// ReSharper disable CollectionNeverUpdated.Global

// ReSharper disable MemberCanBePrivate.Global

namespace Pterodactyl.NETSDK;

public class PterodactylClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly string _apiKey;

    public PterodactylClient(string baseUrl, string apiKey)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("Accept", "Application/vnd.pterodactyl.v1+json");
    }

    #region Client API

    public async Task<List<Server?>> ListServersAsyncClient()
    {
        var servers = new List<Server?>();
        var page = 1;
        var hasMore = true;

        while (hasMore)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/servers?page={page}");
            var result = await HandleResponse<PaginatedResult<ServerResponse>>(response);

            servers.AddRange(result?.Data.Select(r => r.Server) ?? Array.Empty<Server?>());

            if (result != null) hasMore = result.Meta.Pagination.CurrentPage < result.Meta.Pagination.TotalPages;
            page++;
        }

        return servers;
    }

    public async Task<Account?> GetAccountDetailsAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/client/account");
        return await HandleResponse<Account>(response);
    }

    public async Task<TwoFactorDetails?> GetTwoFactorDetailsAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/client/account/two-factor");
        return await HandleResponse<TwoFactorDetails>(response);
    }

    public async Task<TwoFactorEnableResponse?> EnableTwoFactorAsync(string code)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { code }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/client/account/two-factor", content);
        return await HandleResponse<TwoFactorEnableResponse>(response);
    }

    public async Task DisableTwoFactorAsync(string password)
    {
        var requestBody = new { password };
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/api/client/account/two-factor")
        {
            Content = content
        };

        var response = await _httpClient.SendAsync(request);

        await HandleResponse(response);
    }

    public async Task UpdateEmailAsync(string email, string password)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { email, password }), Encoding.UTF8,
            "application/json");
        var response = await _httpClient.PutAsync($"{_baseUrl}/api/client/account/email", content);
        await HandleResponse(response);
    }

    public async Task UpdatePasswordAsync(string currentPassword, string newPassword, string confirmPassword)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            current_password = currentPassword,
            password = newPassword,
            password_confirmation = confirmPassword
        }), Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/client/account/password", content);
        await HandleResponse(response);
    }

    public async Task<List<ApiKey>?> ListApiKeysAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/client/account/api-keys");
        var result = await HandleResponse<PaginatedResult<ApiKeyResponse>>(response);
        return result?.Data.Select(r => r.ApiKey).ToList();
    }

    public async Task<ApiKeyWithSecret?> CreateApiKeyAsync(string description, List<string>? allowedIps = null)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            description,
            allowed_ips = allowedIps ?? []
        }), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/client/account/api-keys", content);
        var result = await HandleResponse<ApiKeyCreateResponse>(response);
        return result?.ApiKeyWithSecret;
    }

    public async Task DeleteApiKeyAsync(string identifier)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/client/account/api-keys/{identifier}");
        await HandleResponse(response);
    }

    public async Task<Server?> GetServerDetailsAsync(string serverId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/client/servers/{serverId}");
        return await HandleResponse<Server>(response);
    }

    public async Task SendCommandAsync(string serverId, string command)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { command }), Encoding.UTF8,
            "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/client/servers/{serverId}/command", content);
        await HandleResponse(response);
    }

    #endregion

    #region Application API

    #region Users

    public async Task<PaginatedResult<UserResponse>?> ListUsersAsync(int page = 1, int perPage = 50)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["page"] = page.ToString();
        query["per_page"] = perPage.ToString();

        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/users?{query}");
        return await HandleResponse<PaginatedResult<UserResponse>>(response);
    }

    public async Task<ApplicationUser?> GetUserAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/users/{userId}");
        var result = await HandleResponse<SingleUserResponse>(response);
        return result?.User;
    }

    public async Task<ApplicationUser?> GetUserByExternalIdAsync(string externalId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/users/external/{externalId}");
        var result = await HandleResponse<SingleUserResponse>(response);
        return result?.User;
    }

    public async Task<ApplicationUser?> CreateUserAsync(UserCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/application/users", content);
        var result = await HandleResponse<SingleUserResponse>(response);
        return result?.User;
    }

    public async Task<ApplicationUser?> UpdateUserAsync(int userId, Action<UserUpdateRequest> updateAction)
    {
        var currentUser = await GetUserAsync(userId);

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
        var response = await _httpClient.PatchAsync($"{_baseUrl}/api/application/users/{userId}", content);
        var result = await HandleResponse<SingleUserResponse>(response);
        return result?.User;
    }

    public async Task DeleteUserAsync(int userId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/application/users/{userId}");
        await HandleResponse(response);
    }

    #endregion

    #region Nodes

    public async Task<PaginatedResult<Node>?> ListNodesAsync(int page = 1, int perPage = 50)
    {
        var response =
            await _httpClient.GetAsync($"{_baseUrl}/api/application/nodes?page={page}&per_page={perPage}");
        return await HandleResponse<PaginatedResult<Node>>(response);
    }

    public async Task<Node?> GetNodeAsync(int nodeId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/nodes/{nodeId}");
        return await HandleResponse<Node>(response);
    }

    public async Task<NodeConfiguration?> GetNodeConfigurationAsync(int nodeId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/nodes/{nodeId}/configuration");
        return await HandleResponse<NodeConfiguration>(response);
    }


    public async Task<Node?> CreateNodeAsync(NodeCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/application/nodes", content);
        return await HandleResponse<Node>(response);
    }

    public async Task<Node?> UpdateNodeAsync(int nodeId, NodeUpdateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync($"{_baseUrl}/api/application/nodes/{nodeId}", content);
        return await HandleResponse<Node>(response);
    }

    public async Task DeleteNodeAsync(int nodeId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/application/nodes/{nodeId}");
        await HandleResponse(response);
    }

    public async Task<List<Allocation>?> GetNodeAllocationsAsync(int nodeId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/nodes/{nodeId}/allocations");
        var result = await HandleResponse<PaginatedResult<AllocationResponse>>(response);
        return result?.Data.Select(r => r.Allocation).ToList();
    }

    #endregion

    #region Servers

    public async Task<PaginatedResult<ApplicationServer>?> ListServersAsync(int page = 1, int perPage = 50)
    {
        var response =
            await _httpClient.GetAsync($"{_baseUrl}/api/application/servers?page={page}&per_page={perPage}");
        return await HandleResponse<PaginatedResult<ApplicationServer>>(response);
    }

    public async Task<Server?> GetServerAsync(int serverId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/servers/{serverId}");
        var result = await HandleResponse<ServerResponse>(response);
        return result?.Server;
    }

    public async Task<Server?> GetServerByExternalIdAsync(string externalId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/servers/external/{externalId}");
        var result = await HandleResponse<ServerResponse>(response);
        return result?.Server;
    }

    public async Task<ApplicationServer?> CreateServerAsync(ServerCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/application/servers", content);
        var result = await HandleResponse<ApplicationServerResponse>(response);
        return result?.ApplicationServer;
    }

    public async Task<ApplicationServer?> UpdateServerDetailsAsync(int serverId, ServerDetailsUpdateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response =
            await _httpClient.PatchAsync($"{_baseUrl}/api/application/servers/{serverId}/details", content);
        return await HandleResponse<ApplicationServer>(response);
    }

    public async Task SuspendServerAsync(int serverId)
    {
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/application/servers/{serverId}/suspend", null);
        await HandleResponse(response);
    }

    public async Task DeleteServerAsync(int serverId, bool force = false)
    {
        var url = $"{_baseUrl}/api/application/servers/{serverId}" + (force ? "/force" : "");
        var response = await _httpClient.DeleteAsync(url);
        await HandleResponse(response);
    }

    #endregion

    #region Locations

    public async Task<PaginatedResult<LocationResponse>?> ListLocationsAsync(int page = 1, int perPage = 50)
    {
        var response = await _httpClient.GetAsync(
            $"{_baseUrl}/api/application/locations?page={page}&per_page={perPage}"
        );
        return await HandleResponse<PaginatedResult<LocationResponse>>(response);
    }

    public async Task<Location?> CreateLocationAsync(LocationCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/api/application/locations", content);
        var result = await HandleResponse<LocationResponse>(response);
        return result?.Location;
    }

    public async Task<Location?> UpdateLocationAsync(int locationId, LocationUpdateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync($"{_baseUrl}/api/application/locations/{locationId}", content);
        var result = await HandleResponse<LocationResponse>(response);
        return result?.Location;
    }

    public async Task DeleteLocationAsync(int locationId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/application/locations/{locationId}");
        await HandleResponse(response);
    }

    #endregion

    #region Databases 一般弃用

    public async Task<PaginatedResult<ServerDatabase>?> ListServerDatabasesAsync(int serverId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/servers/{serverId}/databases");
        return await HandleResponse<PaginatedResult<ServerDatabase>>(response);
    }

    public async Task<ServerDatabase?> CreateServerDatabaseAsync(int serverId, DatabaseCreateRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response =
            await _httpClient.PostAsync($"{_baseUrl}/api/application/servers/{serverId}/databases", content);
        return await HandleResponse<ServerDatabase>(response);
    }

    public async Task ResetDatabasePasswordAsync(int serverId, int databaseId)
    {
        var response = await _httpClient.PostAsync(
            $"{_baseUrl}/api/application/servers/{serverId}/databases/{databaseId}/reset-password", null);
        await HandleResponse(response);
    }

    public async Task DeleteServerDatabaseAsync(int serverId, int databaseId)
    {
        var response = await _httpClient.DeleteAsync(
            $"{_baseUrl}/api/application/servers/{serverId}/databases/{databaseId}");
        await HandleResponse(response);
    }

    #endregion

    #region Nests

    public async Task<PaginatedResult<NestResponse>?> ListNestsAsync(int page = 1, int perPage = 50)
    {
        var response = await _httpClient.GetAsync(
            $"{_baseUrl}/api/application/nests?page={page}&per_page={perPage}"
        );
        return await HandleResponse<PaginatedResult<NestResponse>>(response);
    }

    public async Task<Nest?> GetNestAsync(int nestId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/nests/{nestId}");
        var result = await HandleResponse<NestResponse>(response);
        return result?.Nest;
    }

    public async Task<PaginatedResult<EggResponse>?> ListNestEggsAsync(int nestId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/nests/{nestId}/eggs");
        return await HandleResponse<PaginatedResult<EggResponse>>(response);
    }

    public async Task<Egg?> GetEggAsync(int nestId, int eggId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/application/nests/{nestId}/eggs/{eggId}");
        var result = await HandleResponse<EggResponse>(response);
        return result?.Egg;
    }

    #endregion

    #endregion

    #region Helper Methods

    private async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new PterodactylApiException(
                $"API请求失败，状态码: {response.StatusCode}" +
                response.StatusCode +
                content
            );
        }

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

    private async Task HandleResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new PterodactylApiException(
                $"API request failed with status code {response.StatusCode}: {errorContent}");
        }
    }

    #endregion

    #region Models

    #region Client Models

    public class Server
    {
        [JsonPropertyName("identifier")] public string? Identifier { get; set; }
        [JsonPropertyName("uuid")] public string? Uuid { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("is_suspended")] public bool IsSuspended { get; set; }
        [JsonPropertyName("is_installing")] public bool IsInstalling { get; set; }
        [JsonPropertyName("limits")] public ServerLimits? Limits { get; set; }
        [JsonPropertyName("feature_limits")] public ServerFeatureLimits? FeatureLimits { get; set; }
    }

    public class ServerLimits
    {
        [JsonPropertyName("memory")] public int Memory { get; set; }
        [JsonPropertyName("swap")] public int Swap { get; set; }
        [JsonPropertyName("disk")] public int Disk { get; set; }
        [JsonPropertyName("io")] public int Io { get; set; }
        [JsonPropertyName("cpu")] public int Cpu { get; set; }
    }

    public class ServerFeatureLimits
    {
        [JsonPropertyName("databases")] public int Databases { get; set; }
        [JsonPropertyName("allocations")] public int Allocations { get; set; }
        [JsonPropertyName("backups")] public int Backups { get; set; }
    }

    public class Account
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("admin")] public bool Admin { get; set; }
        [JsonPropertyName("username")] public string? Username { get; set; }
        [JsonPropertyName("email")] public string? Email { get; set; }
        [JsonPropertyName("first_name")] public string? FirstName { get; set; }
        [JsonPropertyName("last_name")] public string? LastName { get; set; }
        [JsonPropertyName("language")] public string? Language { get; set; }
    }

    public class TwoFactorDetails
    {
        [JsonPropertyName("image_url_data")] public string? ImageUrlData { get; set; }
    }

    public class TwoFactorEnableResponse
    {
        [JsonPropertyName("tokens")] public List<string>? Tokens { get; set; }
        [JsonPropertyName("allowed_ips")] public List<string>? AllowedIps { get; set; }
        [JsonPropertyName("last_used_at")] public DateTime? LastUsedAt { get; set; }
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    }

    public class ApiKeyCreateResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }

        [JsonPropertyName("attributes")] public ApiKeyWithSecret? ApiKeyWithSecret { get; set; }
    }

    public class ApiKeyWithSecret : ApiKey
    {
        [JsonPropertyName("token")] public string? SecretToken { get; set; }
    }

    public class WebSocketDetails
    {
        [JsonPropertyName("token")] public string? Token { get; set; }
        [JsonPropertyName("socket")] public string? Socket { get; set; }
    }

    public class ResourceUsage
    {
        [JsonPropertyName("current_state")] public string? CurrentState { get; set; }
        [JsonPropertyName("is_suspended")] public bool IsSuspended { get; set; }
        [JsonPropertyName("resources")] public ResourceStats? Resources { get; set; }
    }

    public class ResourceStats
    {
        [JsonPropertyName("memory_bytes")] public long MemoryBytes { get; set; }
        [JsonPropertyName("cpu_absolute")] public long CpuAbsolute { get; set; }
        [JsonPropertyName("disk_bytes")] public long DiskBytes { get; set; }
        [JsonPropertyName("network_rx_bytes")] public long NetworkRxBytes { get; set; }
        [JsonPropertyName("network_tx_bytes")] public long NetworkTxBytes { get; set; }
    }

    public class Database
    {
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("host")] public DatabaseHost? Host { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("username")] public string? Username { get; set; }
        [JsonPropertyName("connections_from")] public string? ConnectionsFrom { get; set; }
        [JsonPropertyName("max_connections")] public int MaxConnections { get; set; }
        [JsonPropertyName("password")] public DatabasePassword? Password { get; set; }
    }

    public class DatabaseHost
    {
        [JsonPropertyName("address")] public string? Address { get; set; }
        [JsonPropertyName("port")] public int Port { get; set; }
    }

    public class DatabasePassword
    {
        [JsonPropertyName("password")] public string? Password { get; set; }
    }

    public class FileObject
    {
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("mode")] public string? Mode { get; set; }
        [JsonPropertyName("size")] public long Size { get; set; }
        [JsonPropertyName("is_file")] public bool IsFile { get; set; }
        [JsonPropertyName("is_symlink")] public bool IsSymlink { get; set; }
        [JsonPropertyName("is_editable")] public bool IsEditable { get; set; }
        [JsonPropertyName("mimetype")] public string? Mimetype { get; set; }
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("modified_at")] public DateTime ModifiedAt { get; set; }
    }

    public class ApiKeyResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }

        [JsonPropertyName("attributes")] public ApiKey? ApiKey { get; set; }
    }

    public class ApiKey
    {
        [JsonPropertyName("identifier")] public string? Identifier { get; set; }

        [JsonPropertyName("description")] public string? Description { get; set; }

        [JsonPropertyName("allowed_ips")] public List<string>? AllowedIps { get; set; }

        [JsonPropertyName("last_used_at")] public DateTime? LastUsedAt { get; set; }

        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    }

    public class SignedUrl
    {
        [JsonPropertyName("url")] public string? Url { get; set; }
    }

    #endregion

    #region Application Models

    public class ApplicationServerResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }

        [JsonPropertyName("attributes")] public ApplicationServer? ApplicationServer { get; set; }
    }

    public class ApplicationServer
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("external_id")] public string? ExternalId { get; set; }
        [JsonPropertyName("uuid")] public string? Uuid { get; set; }
        [JsonPropertyName("identifier")] public string? Identifier { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("suspended")] public bool IsSuspended { get; set; }
        [JsonPropertyName("limits")] public ApplicationServerLimits? Limits { get; set; }
        [JsonPropertyName("feature_limits")] public ApplicationFeatureLimits? FeatureLimits { get; set; }
        [JsonPropertyName("user")] public int UserId { get; set; }
        [JsonPropertyName("node")] public int NodeId { get; set; }
        [JsonPropertyName("allocation")] public int AllocationId { get; set; }
        [JsonPropertyName("nest")] public int NestId { get; set; }
        [JsonPropertyName("egg")] public int EggId { get; set; }
        [JsonPropertyName("container")] public ContainerDetails? Container { get; set; }
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    }

    public class ApplicationServerLimits
    {
        [JsonPropertyName("memory")] public int Memory { get; set; }
        [JsonPropertyName("swap")] public int Swap { get; set; }
        [JsonPropertyName("disk")] public int Disk { get; set; }
        [JsonPropertyName("io")] public int Io { get; set; }
        [JsonPropertyName("cpu")] public int Cpu { get; set; }
    }

    public class ApplicationFeatureLimits
    {
        [JsonPropertyName("databases")] public int Databases { get; set; }
        [JsonPropertyName("backups")] public int Backups { get; set; }
    }

    public class ContainerDetails
    {
        [JsonPropertyName("startup_command")] public string? StartupCommand { get; set; }

        [JsonPropertyName("image")] public string? Image { get; set; }

        [JsonPropertyName("installed")] public int Installed { get; set; }

        [JsonPropertyName("environment")] public Dictionary<string, object>? Environment { get; set; }
    }

    public static class ContainerExtensions
    {
        public static T? GetEnvironmentValue<T>(Dictionary<string, object> env, string key)
        {
            if (env.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }

            return default;
        }
    }

    public class NodeConfiguration
    {
        [JsonPropertyName("debug")] public bool Debug { get; set; }
        [JsonPropertyName("uuid")] public string? Uuid { get; set; }
        [JsonPropertyName("token")] public string? Token { get; set; }
        [JsonPropertyName("api")] public NodeApiConfig? Api { get; set; }
        [JsonPropertyName("system")] public NodeSystemConfig? System { get; set; }
    }

    public class NodeApiConfig
    {
        [JsonPropertyName("host")] public string? Host { get; set; }
        [JsonPropertyName("port")] public int Port { get; set; }
        [JsonPropertyName("ssl")] public NodeSslConfig? Ssl { get; set; }
    }

    public class NodeSslConfig
    {
        [JsonPropertyName("enabled")] public bool Enabled { get; set; }
        [JsonPropertyName("cert")] public string? Certificate { get; set; }
        [JsonPropertyName("key")] public string? Key { get; set; }
    }

    public class NodeSystemConfig
    {
        [JsonPropertyName("data")] public string? DataPath { get; set; }
        [JsonPropertyName("sftp")] public NodeSftpConfig? Sftp { get; set; }
    }

    public class NodeSftpConfig
    {
        [JsonPropertyName("bind_port")] public int Port { get; set; }
    }

    public class LocationResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }

        [JsonPropertyName("attributes")] public Location? Location { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("short")] public string? ShortCode { get; set; }
        [JsonPropertyName("long")] public string? LongName { get; set; }
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    }

    public class ServerDatabase
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("server")] public int ServerId { get; set; }
        [JsonPropertyName("host")] public int HostId { get; set; }
        [JsonPropertyName("database")] public string? DatabaseName { get; set; }
        [JsonPropertyName("username")] public string? Username { get; set; }
        [JsonPropertyName("remote")] public string? RemoteAccess { get; set; }
        [JsonPropertyName("max_connections")] public int MaxConnections { get; set; }
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    }

    public class UserResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }
        [JsonPropertyName("attributes")] public ApplicationUser? ApplicationUser { get; set; }
    }

    public class SingleUserResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }

        [JsonPropertyName("attributes")] public ApplicationUser? User { get; set; }
    }

    public class ApplicationUser
    {
        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("external_id")] public string? ExternalId { get; set; }

        [JsonPropertyName("uuid")] public string? Uuid { get; set; }

        [JsonPropertyName("username")] public string? Username { get; set; }

        [JsonPropertyName("email")] public string? Email { get; set; }

        [JsonPropertyName("first_name")] public string? FirstName { get; set; }

        [JsonPropertyName("last_name")] public string? LastName { get; set; }

        [JsonPropertyName("language")] public string? Language { get; set; }

        [JsonPropertyName("root_admin")] public bool IsRootAdmin { get; set; }

        [JsonPropertyName("2fa")] public bool TwoFactorEnabled { get; set; }

        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    }

    public class Nest
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("uuid")] public string? Uuid { get; set; }
        [JsonPropertyName("author")] public string? Author { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    }

    public class NestResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }

        [JsonPropertyName("attributes")] public Nest? Nest { get; set; }
    }

    public class EggResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }

        [JsonPropertyName("attributes")] public Egg? Egg { get; set; }
    }

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

    public class EggConfig
    {
        [JsonPropertyName("files")] public Dictionary<string, FileConfig>? Files { get; set; }
        [JsonPropertyName("startup")] public StartupConfig? Startup { get; set; }
        [JsonPropertyName("stop")] public string? StopCommand { get; set; }
        [JsonPropertyName("logs")] public List<LogConfig>? Logs { get; set; }
    }

    public class LogConfig
    {
        [JsonPropertyName("custom")] public bool IsCustom { get; set; }
        [JsonPropertyName("location")] public string? Location { get; set; }
    }

    public class FileConfig
    {
        [JsonPropertyName("parser")] public string? Parser { get; set; }
        [JsonPropertyName("find")] public Dictionary<string, object>? FindReplace { get; set; }
    }

    public class StartupConfig
    {
        [JsonPropertyName("done")] public string? CompletionExpression { get; set; }
        [JsonPropertyName("userInteraction")] public List<string>? UserInteractions { get; set; }
    }


    public class EggScript
    {
        [JsonPropertyName("privileged")] public bool Privileged { get; set; }
        [JsonPropertyName("install")] public string? InstallScript { get; set; }
        [JsonPropertyName("entry")] public string? EntryPoint { get; set; }
        [JsonPropertyName("container")] public string? ContainerImage { get; set; }
    }

    public class PaginatedResult<T>
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }

        [JsonPropertyName("data")] public List<T>? Data { get; set; }

        [JsonPropertyName("meta")] public PaginationMeta? Meta { get; set; }
    }

    public class PaginationMeta
    {
        [JsonPropertyName("pagination")] public PaginationDetails? Pagination { get; set; }
    }

    public class PaginationDetails
    {
        [JsonPropertyName("total")] public int Total { get; set; }
        [JsonPropertyName("count")] public int Count { get; set; }
        [JsonPropertyName("per_page")] public int PerPage { get; set; }
        [JsonPropertyName("current_page")] public int CurrentPage { get; set; }
        [JsonPropertyName("total_pages")] public int TotalPages { get; set; }
    }

    public class Node
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("uuid")] public string? Uuid { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("location_id")] public int LocationId { get; set; }
        [JsonPropertyName("fqdn")] public string? Fqdn { get; set; }
        [JsonPropertyName("memory")] public int Memory { get; set; }
        [JsonPropertyName("disk")] public int Disk { get; set; }
        [JsonPropertyName("upload_size")] public int UploadSize { get; set; }
        [JsonPropertyName("daemon_base")] public string? DaemonBase { get; set; }
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    }

    public class Allocation
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("ip")] public string? Ip { get; set; }
        [JsonPropertyName("port")] public int Port { get; set; }
        [JsonPropertyName("notes")] public string? Notes { get; set; }
        [JsonPropertyName("assigned")] public bool IsAssigned { get; set; }
    }

    public class AllocationResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }
        [JsonPropertyName("attributes")] public Allocation? Allocation { get; set; }
    }

    #endregion

    #region Request Models

    public class UserCreateRequest
    {
        [JsonPropertyName("email")] public string? Email { get; set; }
        [JsonPropertyName("username")] public string? Username { get; set; }
        [JsonPropertyName("first_name")] public string? FirstName { get; set; }
        [JsonPropertyName("last_name")] public string? LastName { get; set; }
        [JsonPropertyName("password")] public string? Password { get; set; }
        [JsonPropertyName("root_admin")] public bool IsRootAdmin { get; set; }
    }

    public class UserUpdateRequest : UserCreateRequest
    {
        [JsonPropertyName("language")] public string? Language { get; set; }
    }

    public class NodeCreateRequest
    {
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("location_id")] public int LocationId { get; set; }
        [JsonPropertyName("fqdn")] public string? Fqdn { get; set; }
        [JsonPropertyName("scheme")] public string? Scheme { get; set; }
        [JsonPropertyName("memory")] public int Memory { get; set; }

        [JsonPropertyName("memory_overallocate")]
        public int MemoryOverallocation { get; set; }

        [JsonPropertyName("disk")] public int Disk { get; set; }

        [JsonPropertyName("disk_overallocate")]
        public int DiskOverallocation { get; set; }

        [JsonPropertyName("upload_size")] public int UploadSize { get; set; }
        [JsonPropertyName("daemon_sftp")] public int SftpPort { get; set; }
        [JsonPropertyName("daemon_listen")] public int DaemonPort { get; set; }
    }

    public class NodeUpdateRequest : NodeCreateRequest
    {
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("behind_proxy")] public bool BehindProxy { get; set; }
        [JsonPropertyName("maintenance_mode")] public bool MaintenanceMode { get; set; }
    }

    public class ServerCreateRequest
    {
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("user")] public int UserId { get; set; }
        [JsonPropertyName("egg")] public int EggId { get; set; }
        [JsonPropertyName("node")] public int NodeId { get; set; }
        
        [JsonPropertyName("allocation")] public AllocationSettings Allocation { get; set; } = new();
        
        [JsonPropertyName("docker_image")] public string? DockerImage { get; set; }
        [JsonPropertyName("startup")] public string? StartupCommand { get; set; }
        [JsonPropertyName("environment")] public Dictionary<string, string>? Environment { get; set; }
        [JsonPropertyName("limits")] public ApplicationServerLimits? Limits { get; set; }
        [JsonPropertyName("feature_limits")] public ApplicationFeatureLimits? FeatureLimits { get; set; }
    }

    public class AllocationSettings
    {
        [JsonPropertyName("default")] public int DefaultAllocation { get; set; }

        [JsonPropertyName("additional")] public List<int> AdditionalAllocations { get; set; } = new List<int>();
    }

    public class ServerDetailsUpdateRequest
    {
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("user")] public int UserId { get; set; }
        [JsonPropertyName("external_id")] public string? ExternalId { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
    }

    public class ServerResponse
    {
        [JsonPropertyName("object")] public string? ObjectType { get; set; }

        [JsonPropertyName("attributes")] public Server? Server { get; set; }
    }

    public class LocationCreateRequest
    {
        [JsonPropertyName("short")] public string? ShortCode { get; set; }
        [JsonPropertyName("long")] public string? LongName { get; set; }
    }

    public class LocationUpdateRequest : LocationCreateRequest
    {
    }

    public class DatabaseCreateRequest
    {
        [JsonPropertyName("database")] public string? DatabaseName { get; set; }
        [JsonPropertyName("remote")] public string? RemoteAccess { get; set; }
        [JsonPropertyName("host")] public int HostId { get; set; }
    }

    #endregion

    #endregion

    public class PterodactylApiException(string message) : Exception(message);
}