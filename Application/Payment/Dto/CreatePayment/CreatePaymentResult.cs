namespace Application.Payment.Dto.CreatePayment;

/// <summary>
/// Result of payment link creation
/// </summary>
public readonly record struct CreatePaymentResult
{
    /// <summary>
    /// Operation, connected with payment link
    /// </summary>
    public Guid OperationId { get; init; }
    /// <summary>
    /// Payment link
    /// </summary>
    public string PaymentUrl { get; init; }
    /// <summary>
    /// Operation creation date in our system
    /// </summary>
    public DateTime CreatedAt { get; init; }
}