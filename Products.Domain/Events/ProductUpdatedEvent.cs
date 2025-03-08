using Products.Domain.Shared;
using Products.Domain.ValueObjects;

namespace Products.Domain.Events;

public class ProductUpdatedEvent : BaseEvent
{
    public ProductId ProductId { get; }
    public string Name { get; }
    public Price Price { get; }
    public Stock Stock { get; }

    public ProductUpdatedEvent(ProductId productId, string name, Price price, Stock stock)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        Stock = stock;
    }
}