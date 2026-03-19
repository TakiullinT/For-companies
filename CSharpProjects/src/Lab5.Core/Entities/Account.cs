using Core.ResultInfo;

namespace Core.Entities;

public class Account
{
    public Guid Id { get; private set; }

    public MoneyState? MoneyBalance { get; private set; }

    public string Pin { get; private set; }

    private readonly List<Operation> _operationsHistory;

    public IReadOnlyCollection<Operation> OperationsHistory => _operationsHistory.AsReadOnly();

    public Account(Guid id, string pin)
    {
        Id = id;
        MoneyBalance = new MoneyState(0);
        _operationsHistory = new List<Operation>();
        Pin = pin;
    }

    public Result Deposit(MoneyState amount)
    {
        if (amount is null) return Result.Fail("Сумма не может быть null!");
        MoneyBalance = MoneyBalance?.Add(amount);
        _operationsHistory.Add(new Operation("Deposit", amount, DateTime.UtcNow));
        return Result.Success();
    }

    public Result Withdraw(MoneyState amount)
    {
        if (amount is null) return Result.Fail("Сумма не может быть null!");
        if (amount.Amount <= 0) return Result.Fail("Сумма должна быть больше нуля!");

        if (MoneyBalance is null) return Result.Fail("Баланс не определён");
        if (MoneyBalance.Amount < amount.Amount) return Result.Fail("Недостаточно средств");

        MoneyBalance = MoneyBalance.Subtract(amount);
        _operationsHistory.Add(new Operation("Withdraw", amount, DateTime.UtcNow));
        return Result.Success();
    }
}