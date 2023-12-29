using System.Text.Json;

namespace Notification.Sender.Job.Core;

public static class StringUtils
{
    static JsonSerializerOptions Options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static T Deserializer<T>(this string value)
    {
        return JsonSerializer.Deserialize<T>(value, Options);
    }

    public static string Serializer(this object value)
    {
        return JsonSerializer.Serialize(value, Options);
    }
}
