using System.ComponentModel.DataAnnotations;
using Domain.Operations.Enums;

namespace Presentation.Controllers.Users.Dto.Operations;

public sealed class GetOperationsRequest
{
    public Guid? Cursor { get; init; }
    public DateTime? CursorDate { get; init; }
    public OperationStatus? Status { get; init; }
    public PaymentMethod? PaymentMethod { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}