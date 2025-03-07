namespace Products.Domain.ValueObjects;

public record Price
{
    public decimal Value { get; }

    private Price() { }
    
    public Price(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value.ToString("C");
}