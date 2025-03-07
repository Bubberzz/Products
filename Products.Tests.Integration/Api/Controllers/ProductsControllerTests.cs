using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Products.Application.Products.Commands;
using Products.Application.Products.Responses;

namespace Products.Tests.Integration.Api.Controllers;

public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreatedProduct()
    {
        var product = new CreateProductCommand { Name = "Test Product", Price = 100.50m, Stock = 10 };
        var response = await _client.PostAsJsonAsync("/api/v1/products", product);

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server returned 500: {errorContent}");
        }

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    
        var createdProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();
        createdProduct.Should().NotBeNull();
        createdProduct.Name.Should().Be("Test Product");
        createdProduct.Price.Should().Be(100.50m);
        createdProduct.Stock.Should().Be(10);
    }

}