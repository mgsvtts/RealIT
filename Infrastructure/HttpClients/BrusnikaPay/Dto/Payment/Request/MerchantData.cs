using System.Text.Json.Serialization;

namespace Infrastructure.HttpClients.BrusnikaPay.Dto.Payment.Request;

public sealed class MerchantData
{
    [JsonPropertyName("webHook")]
    public string Webhook { get; init; } = null!;
}