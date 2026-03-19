namespace Core.Entities;

public class Operation
{
    public string OperationType { get; private set; }

    public MoneyState Amount { get; private set; }

    public DateTime Timestamp { get; private set; }

    public Operation(string operationType, MoneyState amount, DateTime timestamp)
    {
        OperationType = operationType;
        Amount = amount;
        Timestamp = timestamp;
    }
}