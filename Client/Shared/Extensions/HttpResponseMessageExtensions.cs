using System.Text.Json;
using Client.Shared.Models;

namespace Client.Shared.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<ErrorDetails?> GetErrorDetailsAsync(this HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ErrorDetails>(body);
    }
}
