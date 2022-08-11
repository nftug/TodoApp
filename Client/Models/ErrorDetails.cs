using System.Text.Json.Serialization;

namespace Client.Models;

public class ErrorDetails
{
    [JsonPropertyName("errors")]
    public IDictionary<string, string[]>? Errors { get; set; }
}
