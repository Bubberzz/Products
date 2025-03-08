using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Products.Application.Products.Commands;
using Products.Application.Products.Handlers;
using Products.Domain.Entities;
using Products.Domain.Exceptions;
using Products.Domain.Interfaces;
using Products.Domain.ValueObjects;

namespace Products.Tests.Unit.Application.Products.Handlers;

public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<UpdateProductCommandHandler>> _mockLogger;
    private readonly UpdateProductCommandHandler _handler;

    public UpdateProductCommandHandlerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<UpdateProductCommandHandler>>();

        _handler = new UpdateProductCommandHandler(
            _mockRepository.Object,
            _mockUnitOfWork.Object,
            _mockMediator.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ProductExists_UpdatesProduct()
    {
        var productId = ProductId.New();
        var product = new Product(productId, "Test Product", new Price(10.5m), new Stock(5));
        var command = new UpdateProductCommand { Id = productId, Name = "Updated Product", Price = 15m, Stock = 10 };

        _mockRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _mockRepository.Verify(repo => repo.UpdateAsync(It.Is<Product>(p => p.Name == command.Name && p.Price.Value == command.Price && p.Stock.Value == command.Stock)), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        _mockMediator.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProductDoesNotExist_ThrowsNotFoundException()
    {
        var productId = ProductId.New();
        var command = new UpdateProductCommand { Id = productId, Name = "Updated Product", Price = 15m, Stock = 10 };
        
        _mockRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _mockMediator.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
