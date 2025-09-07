using System.Net;
using Domain.Operations.Enums;

namespace Application.Payment.Dto.CreatePayment;

/// <summary>
/// Request for payment link creation
/// </summary>
public readonly record struct CreatePaymentDto
{
    /// <summary>
    /// Amount of operation
    /// </summary>
    public required decimal Amount { get; init; }
    /// <summary>
    /// Optional payment method, if not present, will be chosen automatically 
    /// </summary>
    public required PaymentMethod? PaymentMethod { get; init; }
    /// <summary>
    /// User of the operation
    /// </summary>
    public required Guid UserId { get; init; }
    /// <summary>
    /// Optional IP address of the user
    /// </summary>
    public required IPAddress? UserIp { get; init; }
}