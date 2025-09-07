using Application.Payment;
using Application.Payment.Dto;
using Application.Payment.Dto.UpdateOperation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Webhooks.Dto;

namespace Presentation.Controllers.Webhooks;

//I did not get a single webhook, however here is an example
[Route("webhook")]
public sealed class WebhookController(IPaymentService _paymentService) : ControllerBase
{
    [HttpPost]
    public async Task Catch([FromBody] WebhookRequest request,CancellationToken token)
    {
        await _paymentService.UpdateOperationAsync(new UpdateOperationDto
        {
            OperationId = Guid.Parse(request.IdTransactionMerchant),
            Amount = request.Amount,
            Commission = request.AmountComission,
            NewStatus = request.Status,
        }, token);
    }
}