using MediatR;
using Products.Domain.ValueObjects;

namespace Products.Application.Products.Commands;

public class UpdateProductCommand : IRequest
{
    public ProductId Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
