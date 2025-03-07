using Microsoft.Extensions.Logging;
using Moq;
using Products.Application.Products.Commands;
using Products.Application.Products.Handlers;
using Products.Domain.Entities;
using Products.Domain.Exceptions;
using Products.Domain.Interfaces;
using Products.Domain.ValueObjects;
using MediatR;

namespace Products.Tests.Unit.Application.Products.Handlers;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<DeleteProductCommandHandler>> _mockLogger;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandHandlerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<DeleteProductCommandHandler>>();

        _handler = new DeleteProductCommandHandler(
            _mockRepository.Object, 
            _mockUnitOfWork.Object, 
            _mockMediator.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ProductExists_DeletesProduct()
    {
        var product = new Product("Test Product", new Price(10.5m), new Stock(5)) { Id = 1 };
        var command = new DeleteProductCommand { Id = 1 };

        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        _mockMediator.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProductDoesNotExist_ThrowsNotFoundException()
    {
        var command = new DeleteProductCommand { Id = 1 };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _mockMediator.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
