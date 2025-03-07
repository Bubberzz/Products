using Products.Domain.Shared;

namespace Products.Domain.Events;

public class ProductCreatedEvent : BaseEvent
{
    public int ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int Stock { get; }

    public ProductCreatedEvent(int productId, string name, decimal price, int stock)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        Stock = stock;
    }
}