namespace Core.Entities;

public class Pin
{
    public string Value { get; private set; }

    public Pin(string value)
    {
        Value = value;
    }

    public override bool Equals(object? obj) => obj is Pin pin && Value == pin.Value;

    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);
}