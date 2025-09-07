using System.Text.Json.Serialization;
using Domain.Operations.Enums;

namespace Presentation.Controllers.Webhooks.Dto;

public sealed class WebhookRequest
{
    public Guid Id { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime DateUpdated { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public OperationStatus Status { get; set; }

    public string IdTransactionMerchant { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal AmountComission { get; set; }

    public decimal AmountInCurrencyBalance { get; set; }

    public decimal AmountComissionInCurrencyBalance { get; set; }

    public decimal ExchangeRate { get; set; }
}