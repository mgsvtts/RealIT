using Domain.Operations.Enums;

namespace Application.Payment.Dto.UpdateOperation;

/// <summary>
/// Request for operation update
/// </summary>
public readonly record struct UpdateOperationDto
{
    /// <summary>
    /// Operation to update
    /// </summary>
    public required Guid OperationId { get; init; }
    /// <summary>
    /// New status of operation
    /// </summary>
    public required OperationStatus NewStatus { get; init; }
    /// <summary>
    /// New amount of operation
    /// </summary>
    public required decimal Amount { get; init; }
    /// <summary>
    /// New commission of operation
    /// </summary>
    public required decimal Commission { get; init; }
}