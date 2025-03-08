using System.Text.Json.Serialization;
using Products.Domain.Shared;
using Products.Domain.ValueObjects;

namespace Products.Domain.Entities;

public class Product : IAggregateRoot
{
    public ProductId Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; private set; } = string.Empty;

    public Price Price { get; private set; }
    public Stock Stock { get; private set; }
    
    private Product() { }
    
    public Product(ProductId id, string name, Price price, Stock stock)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Stock = stock ?? throw new ArgumentNullException(nameof(stock));
    }
    
    public void UpdateDetails(string name, Price price, Stock stock)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Stock = stock ?? throw new ArgumentNullException(nameof(stock));
    }
}