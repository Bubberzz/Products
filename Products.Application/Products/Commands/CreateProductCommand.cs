using MediatR;
using Products.Domain.ValueObjects;

namespace Products.Application.Products.Commands;

public class CreateProductCommand : IRequest<ProductId>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}