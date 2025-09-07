using Application.Payment.Dto.CreatePayment;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.Payments.Dto;

public readonly record struct CreatePaymentResponse
{
    [SwaggerSchema("Operation id in our system")]
    public required Guid OperationId { get; init; }

    [SwaggerSchema("Payment link")]
    public required string PaymentUrl { get; init; }
    
    [SwaggerSchema("Date of creation in our system")]
    public required DateTime CreatedAt { get; init; }

    public static CreatePaymentResponse FromResult(CreatePaymentResult result)
    {
        return new CreatePaymentResponse
        {
            CreatedAt = result.CreatedAt,
            PaymentUrl = result.PaymentUrl,
            OperationId = result.OperationId,
        };
    }
}