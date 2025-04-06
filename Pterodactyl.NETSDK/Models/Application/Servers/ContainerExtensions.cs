namespace Pterodactyl.NETSDK.Models.Application.Servers;

public static class ContainerExtensions
{
    public static T? GetEnvironmentValue<T>(Dictionary<string, object> env, string key)
    {
        if (env.TryGetValue(key, out var value) && value is T typedValue) return typedValue;

        return default;
    }
}