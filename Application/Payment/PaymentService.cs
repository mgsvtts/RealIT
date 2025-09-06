using Application.Payment.CreatePayment;
using Infrastructure.HttpClients;

namespace Application.Payment;

public interface IPaymentService
{
    ValueTask CreateAsync(CreatePaymentRequest request, CancellationToken token);
}

public sealed class PaymentService(IBrusnikaPayHttpClient _httpClient) : IPaymentService
{
    public async ValueTask CreateAsync(CreatePaymentRequest request, CancellationToken token)
    {
        
    }
}