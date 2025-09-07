namespace Infrastructure.HttpClients.BrusnikaPay.Dto.BankItem;

public readonly record struct BankItem
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}