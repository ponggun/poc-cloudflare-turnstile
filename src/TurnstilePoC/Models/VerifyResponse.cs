using System.Text.Json.Serialization;

namespace TurnstilePoC.Models;

public record VerifyResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; init; }
    
    [JsonPropertyName("error-codes")]
    public string[]? ErrorCodes { get; init; }
    
    [JsonPropertyName("hostname")]
    public string? Hostname { get; init; }
    
    [JsonPropertyName("challenge_ts")]
    public string? ChallengeTs { get; init; }
    
    [JsonPropertyName("cdata")]
    public string? CData { get; init; }
}