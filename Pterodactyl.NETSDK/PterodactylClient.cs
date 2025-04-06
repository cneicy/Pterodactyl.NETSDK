using System.Net.Http.Headers;

namespace Pterodactyl.NETSDK;

public abstract class PterodactylClient
{
    protected readonly HttpClient HttpClient;
    protected readonly string BaseUrl;
    protected readonly string ApiKey;

    protected PterodactylClient(string baseUrl, string apiKey)
    {
        BaseUrl = baseUrl.TrimEnd('/');
        ApiKey = apiKey;
        HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpClient.DefaultRequestHeaders.Add("Accept", "Application/vnd.pterodactyl.v1+json");
    }
}