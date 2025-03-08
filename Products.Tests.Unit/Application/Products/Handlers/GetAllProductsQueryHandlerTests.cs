using AutoMapper;
using FluentAssertions;
using Moq;
using Products.Application.Products.Handlers;
using Products.Application.Products.Queries;
using Products.Application.Products.Responses;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Domain.ValueObjects;

namespace Products.Tests.Unit.Application.Products.Handlers;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();

        _handler = new GetAllProductsQueryHandler(
            _mockRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ProductsExist_ReturnsProductList()
    {
        var products = new List<Product>
        {
            new(ProductId.New(), "Product 1", new Price(10.5m), new Stock(5)),
            new(ProductId.New(), "Product 2", new Price(15m), new Stock(10))
        };

        var productResponses = new List<ProductResponse>
        {
            new() { Id = products[0].Id.Value, Name = "Product 1", Price = 10.5m, Stock = 5 },
            new() { Id = products[1].Id.Value, Name = "Product 2", Price = 15m, Stock = 10 }
        };

        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);
        _mockMapper.Setup(m => m.Map<IEnumerable<ProductResponse>>(products)).Returns(productResponses);

        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(productResponses);
    }

    [Fact]
    public async Task Handle_NoProductsExist_ReturnsEmptyList()
    {
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Product>());
        _mockMapper.Setup(m => m.Map<IEnumerable<ProductResponse>>(It.IsAny<List<Product>>()))
                   .Returns(new List<ProductResponse>());

        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
