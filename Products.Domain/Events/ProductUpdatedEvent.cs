using Products.Domain.Shared;

namespace Products.Domain.Events;

public class ProductUpdatedEvent : BaseEvent
{
    public int ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int Stock { get; }

    public ProductUpdatedEvent(int productId, string name, decimal price, int stock)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        Stock = stock;
    }
}