using System.Text.Json.Serialization;
using Domain.Operations.Enums;

namespace Infrastructure.HttpClients.BrusnikaPay.Dto.Payment;

public readonly record struct PaymentResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    
    [JsonPropertyName("dateAdded")]
    public DateTime DateAdded { get; init; }
    
    [JsonPropertyName("dateUpdated")]
    public DateTime DateUpdated { get; init; }
    
    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public OperationStatus Status { get; init; }
    
    [JsonPropertyName("idTransactionMerchant")]
    public string TransactionId { get; init; }
    
    [JsonPropertyName("amountInitial")]
    public decimal AmountInitial { get; init; }
    
    [JsonPropertyName("amountRandomized")]
    public decimal AmountRandomized { get; init; }
    
    [JsonPropertyName("amount")]
    public decimal Amount { get; init; }
    
    [JsonPropertyName("amountComission")]
    public decimal AmountCommission { get; init; }
    
    [JsonPropertyName("amountInCurrencyBalance")]
    public decimal AmountInCurrencyBalance { get; init; }
    
    [JsonPropertyName("amountComissionInCurrencyBalance")]
    public decimal AmountComissionInCurrencyBalance { get; init; }
    
    [JsonPropertyName("exchangeRate")]
    public decimal ExchangeRate { get; init; }
    
    [JsonPropertyName("linkPaymentForm")]
    public string PaymentLink { get; init; }
}