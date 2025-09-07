using Infrastructure.HttpClients.BrusnikaPay.Dto;
using Infrastructure.HttpClients.BrusnikaPay.Dto.BankItem;
using Infrastructure.HttpClients.BrusnikaPay.Dto.Operations;
using Infrastructure.HttpClients.BrusnikaPay.Dto.Payment;
using Infrastructure.HttpClients.BrusnikaPay.Dto.Payment.Request;

namespace Infrastructure.HttpClients.BrusnikaPay;

public interface IBrusnikaPayHttpClient
{
    /// <summary>
    /// Creates payment link when user already chose a payment method
    /// </summary>
    /// <param name="request">Request for creating payment link</param>
    /// <returns>Payment link data</returns>
    Task<BrusnikaPayResponse<PaymentResponse>> FullPaymentAsync(FullPaymentRequest request, CancellationToken token);
    /// <summary>
    /// Creates payment link when user did not choose a payment method
    /// </summary>
    /// <param name="request">Request for creating payment link</param>
    /// <returns>Payment link data</returns>
    Task<BrusnikaPayResponse<PaymentResponse>> PreparePaymentAsync(PreparePaymentRequest request, CancellationToken token);
    /// <summary>
    /// Gets page of user operations
    /// </summary>
    /// <param name="request">Request with page data</param>
    /// <returns>List of performed operations</returns>
    Task<BrusnikaPayResponse<List<OperationItem>>> GetOperationsAsync(GetOperationsRequest request, CancellationToken token);
    /// <summary>
    /// Gets the list of available banks
    /// </summary>
    /// <returns>Bank list</returns>
    Task<BrusnikaPayResponse<List<BankItem>>> GetBankListAsync(CancellationToken token);
}