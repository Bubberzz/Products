using Products.Domain.Shared;
using Products.Domain.ValueObjects;

namespace Products.Domain.Events;

public class ProductDeletedEvent : BaseEvent
{
    public ProductId ProductId { get; }

    public ProductDeletedEvent(ProductId productId)
    {
        ProductId = productId;
    }
}