namespace Products.Domain.ValueObjects;

public record ProductId
{
    public Guid Value { get; }

    public static ProductId New() => new(Guid.NewGuid());
    
    public ProductId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ProductId cannot be empty.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value.ToString();
}