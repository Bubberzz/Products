using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Products.Application.Products.Commands;
using Products.Application.Products.Handlers;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Domain.ValueObjects;

namespace Products.Tests.Unit.Application.Products.Handlers;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CreateProductCommandHandler>> _mockLogger;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CreateProductCommandHandler>>();

        _handler = new CreateProductCommandHandler(
            _mockRepository.Object, 
            _mockUnitOfWork.Object, 
            _mockMediator.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_Should_CreateProduct_And_Return_ProductId()
    {
        var command = new CreateProductCommand { Name = "Test Product", Price = 10.5m, Stock = 5 };
        var productId = ProductId.New();
        var product = new Product(productId, command.Name, new Price(command.Price), new Stock(command.Stock));
        
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Product>()))
            .Callback<Product>(p => p.GetType().GetProperty("Id")?.SetValue(p, productId))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
        
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.Should().NotBeNull();
        result.Should().Be(productId);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
