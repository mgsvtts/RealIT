using System.ComponentModel.DataAnnotations;
using Domain.Operations.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.Users.Dto.Operations;

public sealed class GetOperationsRequest
{
    [SwaggerSchema("Optional cursor value for pagination")]
    public Guid? Cursor { get; init; }
    
    [SwaggerSchema("Optional cursor date for pagination (should be provided with cursor value)")]
    public DateTime? CursorDate { get; init; }
    
    [SwaggerSchema("Optional status to filter operations")]
    public OperationStatus? Status { get; init; }
    
    [SwaggerSchema("Optional payment method to filter operations")]
    public PaymentMethod? PaymentMethod { get; init; }
    
    [SwaggerSchema("Optional start date of operations")]
    public DateTime? DateFrom { get; init; }
    
    [SwaggerSchema("Optional end date of operations")]
    public DateTime? DateTo { get; init; }
}