using Application.Payment;
using Application.Payment.CreatePayment;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Payments;

[ApiController]
[Route("v1/payments")]
public sealed class PaymentController(IPaymentService _service) : ControllerBase
{
    [HttpPost]
    public async Task Create(CreatePaymentRequest request, CancellationToken token)
    {
        
    }
}