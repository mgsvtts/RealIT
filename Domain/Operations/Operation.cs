using Domain.Operations.Enums;
using Domain.Users;

namespace Domain.Operations;

public sealed class Operation
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OperationStatus Status { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }
    public decimal Commission { get; private set; }

    public Operation Create()
    {
        return new Operation();
    }
}