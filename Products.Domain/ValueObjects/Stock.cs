namespace Products.Domain.ValueObjects;

public record Stock
{
    public int Value { get; }

    private Stock() { }
    
    public Stock(int value)
    {
        if (value < 0)
            throw new ArgumentException("Stock cannot be negative.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value.ToString();
}