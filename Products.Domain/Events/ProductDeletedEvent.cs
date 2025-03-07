using Products.Domain.Shared;

namespace Products.Domain.Events;

public class ProductDeletedEvent : BaseEvent
{
    public int ProductId { get; }

    public ProductDeletedEvent(int productId)
    {
        ProductId = productId;
    }
}