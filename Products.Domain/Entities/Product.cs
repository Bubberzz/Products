using System.Text.Json.Serialization;
using Products.Domain.Shared;
using Products.Domain.ValueObjects;

namespace Products.Domain.Entities;

public class Product : IAggregateRoot
{
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; private set; } = string.Empty;

    public Price Price { get; private set; }
    public Stock Stock { get; private set; }
    
    private Product() { }
    
    public Product(string name, Price price, Stock stock)
    {
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