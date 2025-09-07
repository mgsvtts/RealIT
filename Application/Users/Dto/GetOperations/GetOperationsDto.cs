using Domain.Operations.Enums;

namespace Application.Users.Dto.GetOperations;

/// <summary>
/// Request for operation retrieval
/// </summary>
public readonly record struct GetOperationsDto
{
    /// <summary>
    /// Creator of operations
    /// </summary>
    public Guid UserId { get; init; }
    /// <summary>
    /// Optional cursor value (in case of equal dates)
    /// </summary>
    public Guid? Cursor { get; init; }
    /// <summary>
    /// Optional cursor date to sort
    /// </summary>
    public DateTime? CursorDate { get; init; }
    /// <summary>
    /// Optional inclusive start date of the search
    /// </summary>
    public DateTime? DateFrom { get; init; }
    /// <summary>
    /// Optional inclusive end date of the search
    /// </summary>
    public DateTime? DateTo { get; init; }
    /// <summary>
    /// Optional status filter
    /// </summary>
    public OperationStatus? Status { get; init; }
    /// <summary>
    /// Optional payment method filter
    /// </summary>
    public PaymentMethod? PaymentMethod { get; init; }
}