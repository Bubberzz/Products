using MediatR;
using Microsoft.Extensions.Logging;
using Products.Domain.Events;

namespace Products.Application.Products.EventHandlers;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Product Created: ID {notification.ProductId}, Name {notification.Name}, Price {notification.Price}, Stock {notification.Stock}");
        await Task.CompletedTask;
    }
}