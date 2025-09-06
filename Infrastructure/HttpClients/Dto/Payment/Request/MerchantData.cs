using System.Text.Json.Serialization;

namespace Infrastructure.HttpClients.Dto.Payment.Request;

public readonly record struct MerchantData
{
    [JsonPropertyName("webHook")]
    public string Webhook { get; init; }
    
    [JsonPropertyName("redirectGeneralURL")]
    public string GeneralUrl { get; init; }
    
    [JsonPropertyName("redirectSuccessURL")]
    public string SuccessUrl { get; init; }
    
    [JsonPropertyName("redirectFailURL")]
    public string FailUrl { get; init; }
}