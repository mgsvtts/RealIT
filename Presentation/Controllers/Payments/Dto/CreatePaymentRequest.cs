using System.ComponentModel.DataAnnotations;
using Domain.Operations.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.Payments.Dto;

public readonly record struct CreatePaymentRequest
{
    [Required]
    [Range(1, double.MaxValue)]
    [SwaggerSchema("Amount of the transaction")]
    public decimal Amount { get; init; }
    
    [SwaggerSchema("Payment method, will be chosen automatically if not set")]
    public PaymentMethod? PaymentMethod { get; init; }
}