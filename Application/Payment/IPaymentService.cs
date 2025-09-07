using Application.Payment.Dto.CreatePayment;
using Application.Payment.Dto.UpdateOperation;

namespace Application.Payment;

/// <summary>
/// Service for handling payments and operations
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Creates payment link and operation for it
    /// </summary>
    /// <returns>Result with payment link</returns>
    Task<CreatePaymentResult> CreateAsync(CreatePaymentDto request, CancellationToken token);
    
    /// <summary>
    /// Synchronized operations with external system
    /// </summary>
    Task SynchronizeOperationsAsync(CancellationToken token);
    
    /// <summary>
    /// Updates chosen operation, works like HTTP PUT (rewrites existing values)
    /// </summary>
    Task UpdateOperationAsync(UpdateOperationDto request, CancellationToken token);
}