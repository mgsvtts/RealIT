using Application.Payment;
using Application.Payment.Dto;
using Application.Payment.Dto.CreatePayment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Payments.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.Payments;

[Authorize]
[ApiController]
[Route("v1/payments")]
public sealed class PaymentController(IPaymentService _paymentService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates the payment link using BrusnikaPay")]
    public async Task<ActionResult<CreatePaymentResponse>> Create(CreatePaymentRequest request, CancellationToken token)
    {
        var result = await _paymentService.CreateAsync(new CreatePaymentDto
        {
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            UserId = HttpContext.GetUserIdOrThrow(),
            UserIp = HttpContext.Connection.RemoteIpAddress
        }, token);

        return Ok(CreatePaymentResponse.FromResult(result));
    }
}