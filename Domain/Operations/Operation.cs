using System.Data;
using Domain.Operations.Enums;
using Domain.Sort;
using Domain.Users;

namespace Domain.Operations;

public sealed class Operation : ICursorSortable<Guid>
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public OperationStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public decimal Amount { get; private set; }
    public decimal Commission { get; private set; }

    public static Operation New(
        Guid userId,
        decimal amount,
        PaymentMethod paymentMethod)
    {
        return new Operation
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            Amount = amount,
            CreatedAt = DateTime.UtcNow,
            PaymentMethod = paymentMethod,
            Status = OperationStatus.Created,
        };
    }

    public void Update(
        OperationStatus status,
        decimal amount,
        decimal commission)
    {
        Amount = amount;
        Status = status;
        Commission = commission;
        UpdatedAt = DateTime.UtcNow;
    }
}