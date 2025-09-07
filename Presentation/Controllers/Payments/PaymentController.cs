using Application.Payment;
using Application.Payment.Dto;
using Application.Payment.Dto.CreatePayment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Payments.Dto;

namespace Presentation.Controllers.Payments;

[Authorize]
[ApiController]
[Route("v1/payments")]
public sealed class PaymentController(IPaymentService _paymentService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreatePaymentResult>> Create(CreatePaymentRequest request, CancellationToken token)
    {
        var response = await _paymentService.CreateAsync(new CreatePaymentDto
        {
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            UserId = HttpContext.GetUserIdOrThrow(),
            UserIp = HttpContext.Connection.RemoteIpAddress
        }, token);

        return Ok(response);
    }
}