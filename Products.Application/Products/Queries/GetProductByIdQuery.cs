using MediatR;
using Products.Application.Products.Responses;
using Products.Domain.ValueObjects;

namespace Products.Application.Products.Queries;

public class GetProductByIdQuery : IRequest<ProductResponse>
{
    public ProductId Id { get; set; }
}
