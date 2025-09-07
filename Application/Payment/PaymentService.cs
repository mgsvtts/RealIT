using System.ComponentModel.DataAnnotations;
using Application.Payment.Dto;
using Application.Payment.Dto.CreatePayment;
using Application.Payment.Dto.UpdateOperation;
using Domain.Operations;
using Domain.Operations.Enums;
using Domain.Users;
using Infrastructure.Database;
using Infrastructure.HttpClients;
using Infrastructure.HttpClients.BrusnikaPay;
using Infrastructure.HttpClients.BrusnikaPay.Dto;
using Infrastructure.HttpClients.BrusnikaPay.Dto.Operations;
using Infrastructure.HttpClients.BrusnikaPay.Dto.Payment;
using Infrastructure.HttpClients.BrusnikaPay.Dto.Payment.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Application.Payment;

public sealed class PaymentService(
    PaymentDbContext _db,
    ILogger<PaymentService> _logger,
    IBrusnikaPayHttpClient _httpClient,
    MerchantData _merchantData) : IPaymentService
{
    public async Task<CreatePaymentResult> CreateAsync(CreatePaymentDto request, CancellationToken token)
    {
        var user = await GetUserAsync(request.UserId, token);

        var operation = Operation.New(user.Id, request.Amount, request.PaymentMethod ?? PaymentMethod.Unknown);

        var form = await CreatePaymentForm(request, user, operation, token);

        _logger.LogInformation("Got payment details: {@PaymentDetails}", form);

        operation.Update(form.Data.Status, form.Data.Amount, form.Data.AmountCommission);

        _db.Operations.Add(operation);
        
        await _db.SaveChangesAsync(token);

        return new CreatePaymentResult
        {
            OperationId = operation.Id,
            CreatedAt = operation.CreatedAt,
            PaymentUrl = form.Data.PaymentLink
        };
    }

    public async Task SynchronizeOperationsAsync(CancellationToken token)
    {
        var skip = 0;
        const int take = 5; //in real world should be greater, but I don't want to create thousands of operations :)
        
        while (true)
        {
            var operationsTask = _db.Operations
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync(token);

            var brusnikaOperationsTask = _httpClient.GetOperationsAsync(new GetOperationsRequest
            {
                Skip = skip,
                Take = take
            }, token);

            await Task.WhenAll(operationsTask, brusnikaOperationsTask);

            var operations = operationsTask.Result;
            var brusnikaOperations = brusnikaOperationsTask.Result;

            if (brusnikaOperations.Data.Count == 0 || operations.Count == 0)
            {
                break;
            }

            var brusnikaDict = brusnikaOperations.Data.ToDictionary(x => x.IdTransactionMerchant, x => x);

            foreach (var operation in operations)
            {
                if (!brusnikaDict.TryGetValue(operation.Id.ToString(), out var brusnikaOperation))
                {
                    continue;
                }

                if (brusnikaOperation.Status == operation.Status)
                {
                    continue;
                }

                operation.Update(
                    brusnikaOperation.Status,
                    brusnikaOperation.Amount,
                    brusnikaOperation.AmountCommission);
            }
            
            skip += take;
        }

        await _db.SaveChangesAsync(token);
    }

    public async Task UpdateOperationAsync(UpdateOperationDto request, CancellationToken token)
    {
        var operation = await GetOperationAsync(request.OperationId, token);
        
        operation.Update(request.NewStatus, request.Amount, request.Commission);

        await _db.SaveChangesAsync(token);
    }

    private async Task<User> GetUserAsync(Guid userId, CancellationToken token)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId, token);

        if (user is null)
        {
            _logger.LogWarning("User: {UserId} not found", userId);

            throw new ValidationException("User not found");
        }

        return user;
    }
    
    private async Task<Operation> GetOperationAsync(Guid operationId, CancellationToken token)
    {
        var operation = await _db.Operations.FirstOrDefaultAsync(x => x.Id == operationId, token);

        if (operation is null)
        {
            _logger.LogWarning("Operation: {OperationId} not found", operationId);

            throw new ValidationException("Operation not found");
        }

        return operation;
    }

    private async Task<BrusnikaPayResponse<PaymentResponse>> CreatePaymentForm(
        CreatePaymentDto request,
        User user,
        Operation operation,
        CancellationToken token)
    {
        if (request.PaymentMethod is null or PaymentMethod.Unknown)
        {
            return await _httpClient.PreparePaymentAsync(new PreparePaymentRequest
            {
                Amount = request.Amount,
                ClientId = user.Id.ToString(),
                ClientIp = request.UserIp?.MapToIPv4().ToString() ?? "",
                CreatedAt = DateTime.UtcNow,
                TransactionId = operation.Id.ToString(),
                MerchantData = _merchantData
            }, token);
        }

        return await _httpClient.FullPaymentAsync(new FullPaymentRequest
        {
            Amount = request.Amount,
            ClientId = user.Id.ToString(),
            ClientIp = request.UserIp?.MapToIPv4().ToString() ?? "",
            CreatedAt = DateTime.UtcNow,
            PaymentMethod = request.PaymentMethod.Value,
            TransactionId = operation.Id.ToString(),
            MerchantData = _merchantData
        }, token);
    }
}