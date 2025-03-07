using MediatR;

namespace Products.Application.Products.Commands;

public class DeleteProductCommand : IRequest
{
    public int Id { get; set; }
}
