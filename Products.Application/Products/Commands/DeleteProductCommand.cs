using MediatR;
using Products.Domain.ValueObjects;

namespace Products.Application.Products.Commands;

public class DeleteProductCommand : IRequest
{
    public ProductId Id { get; set; }
}
