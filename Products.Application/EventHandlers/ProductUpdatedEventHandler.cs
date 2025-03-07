using MediatR;
using Microsoft.Extensions.Logging;
using Products.Domain.Events;

namespace Products.Application.Products.EventHandlers;

public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
{
    private readonly ILogger<ProductUpdatedEventHandler> _logger;

    public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product Updated: ID {ProductId}, Name: {Name}, Price: {Price}, Stock: {Stock}", 
            notification.ProductId, notification.Name, notification.Price, notification.Stock);

        await Task.CompletedTask;
    }
}