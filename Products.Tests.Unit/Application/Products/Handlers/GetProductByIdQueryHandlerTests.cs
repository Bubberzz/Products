using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Products.Application.Products.Handlers;
using Products.Application.Products.Queries;
using Products.Domain.Entities;
using Products.Domain.Exceptions;
using Products.Domain.Interfaces;
using Products.Domain.ValueObjects;

namespace Products.Tests.Unit.Application.Products.Handlers;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<ILogger<GetProductByIdQueryHandler>> _loggerMock;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<GetProductByIdQueryHandler>>();
        _handler = new GetProductByIdQueryHandler(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ProductExists_ReturnsProductResponse()
    {
        var productId = ProductId.New();
        var product = new Product(productId, "Test Product", new Price(100), new Stock(50));

        _repositoryMock.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(product);

        var query = new GetProductByIdQuery { Id = productId };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(product.Id.Value);
        result.Name.Should().Be(product.Name);
        result.Price.Should().Be(product.Price.Value);
        result.Stock.Should().Be(product.Stock.Value);

        _repositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        _loggerMock.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Fetching product with ID")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ProductDoesNotExist_ThrowsNotFoundException()
    {
        var productId = ProductId.New();

        _repositoryMock.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync((Product?)null);

        var query = new GetProductByIdQuery { Id = productId };

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(query, CancellationToken.None));

        _repositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        _loggerMock.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Warning),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Product with ID")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
}
