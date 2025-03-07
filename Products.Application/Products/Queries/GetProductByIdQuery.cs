using MediatR;
using Products.Application.Products.Responses;

namespace Products.Application.Products.Queries;

public class GetProductByIdQuery : IRequest<ProductResponse>
{
    public int Id { get; set; }
}
