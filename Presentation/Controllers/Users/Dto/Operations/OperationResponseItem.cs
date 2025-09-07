using Domain.Operations;
using Domain.Operations.Enums;
using Domain.Sort;
using Domain.Users;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.Users.Dto.Operations;

public sealed class OperationResponseItem : ICursorSortable<Guid>
{
    [SwaggerSchema("Id of the operation in our system")]
    public required Guid Id { get; init; }
    
    [SwaggerSchema("Id of the operation in BrusnikaPay")]
    public required Guid ExternalId { get; init; }
    
    [SwaggerSchema("Date of operation creation")]
    public required DateTime CreatedAt { get; init; }
    
    [SwaggerSchema("Date of last operation update")]
    public required DateTime? UpdatedAt { get; init; }
    
    [SwaggerSchema("Current status of the operation")]
    public required OperationStatus Status { get; init; }
    
    [SwaggerSchema("Payment method of the operation")]
    public required PaymentMethod PaymentMethod { get; init; }
    
    [SwaggerSchema("Amount of the operation")]
    public required decimal Amount { get; init; }
    
    [SwaggerSchema("Commission of the operation")]
    public required decimal Commission { get; init; }

    public static OperationResponseItem FromOperation(Operation operation)
    {
        return new OperationResponseItem
        {
            Id = operation.Id,
            ExternalId = operation.ExternalId,
            CreatedAt = operation.CreatedAt,
            UpdatedAt = operation.UpdatedAt,
            Status = operation.Status,
            PaymentMethod = operation.PaymentMethod,
            Amount = operation.Amount,
            Commission = operation.Commission
        };
    }
}