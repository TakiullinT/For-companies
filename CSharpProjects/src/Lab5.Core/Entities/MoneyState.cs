namespace Core.Entities;

public class MoneyState
{
    public decimal Amount { get; }

    public MoneyState(decimal amount)
    {
        Amount = amount;
    }

    public MoneyState Add(MoneyState other) => new MoneyState(Amount + other.Amount);

    public MoneyState Subtract(MoneyState other) => new MoneyState(Amount - other.Amount);

    public override bool Equals(object? obj) => obj is MoneyState other && Amount.Equals(other.Amount);

    public override int GetHashCode() => Amount.GetHashCode();
}