using System.Text.Json.Serialization;
using Domain.Operations.Enums;

namespace Infrastructure.HttpClients.BrusnikaPay.Dto.Payment.Request;

public readonly record struct FullPaymentRequest
{
    [JsonPropertyName("clientID")]
    public required string ClientId { get; init; }
    
    [JsonPropertyName("clientIP")]
    public required string ClientIp { get; init; }
    
    [JsonPropertyName("clientDateCreated")]
    public required DateTime CreatedAt { get; init; }
    
    [JsonPropertyName("paymentMethod")]
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public required PaymentMethod PaymentMethod { get; init; }
    
    [JsonPropertyName("idTransactionMerchant")]
    public required string TransactionId { get; init; }
    
    [JsonPropertyName("amount")]
    public required decimal Amount { get; init; }
    
    [JsonPropertyName("integrationMerhcnatData")]
    public required MerchantData MerchantData { get; init; }
}