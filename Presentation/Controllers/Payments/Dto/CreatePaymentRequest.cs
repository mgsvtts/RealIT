using System.ComponentModel.DataAnnotations;
using Domain.Operations.Enums;

namespace Presentation.Controllers.Payments.Dto;

public sealed class CreatePaymentRequest
{
    [Required]
    [Range(1, double.MaxValue)]
    public decimal Amount { get; init; }
    
    public PaymentMethod? PaymentMethod { get; init; }
    
}