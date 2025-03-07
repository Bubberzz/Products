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
    public async Task GetAllProducts_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        var response = await _client.GetAsync("/api/v1/products");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
        products.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreatedProduct()
    {
        var product = new CreateProductCommand { Name = "Test Product", Price = 100.50m, Stock = 10 };
        var response = await _client.PostAsJsonAsync("/api/v1/products", product);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    
        var createdProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();
        createdProduct.Should().NotBeNull();
        createdProduct.Name.Should().Be("Test Product");
        createdProduct.Price.Should().Be(100.50m);
        createdProduct.Stock.Should().Be(10);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        var response = await _client.GetAsync("/api/v1/products/999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
    {
        var product = new CreateProductCommand { Name = "Test Product", Price = 50.00m, Stock = 5 };
        var createResponse = await _client.PostAsJsonAsync("/api/v1/products", product);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();

        var response = await _client.GetAsync($"/api/v1/products/{createdProduct.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var retrievedProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();
        retrievedProduct.Should().NotBeNull();
        retrievedProduct.Id.Should().Be(createdProduct.Id);
        retrievedProduct.Name.Should().Be("Test Product");
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnNoContent_WhenProductExists()
    {
        var product = new CreateProductCommand { Name = "Old Name", Price = 20.00m, Stock = 15 };
        var createResponse = await _client.PostAsJsonAsync("/api/v1/products", product);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();

        var updateCommand = new UpdateProductCommand { Id = createdProduct.Id, Name = "Updated Name", Price = 30.00m, Stock = 20 };
        var response = await _client.PutAsJsonAsync($"/api/v1/products/{createdProduct.Id}", updateCommand);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNoContent_WhenProductExists()
    {
        var product = new CreateProductCommand { Name = "To Be Deleted", Price = 10.00m, Stock = 2 };
        var createResponse = await _client.PostAsJsonAsync("/api/v1/products", product);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();

        var response = await _client.DeleteAsync($"/api/v1/products/{createdProduct.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}