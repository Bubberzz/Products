using MediatR;
using Products.Application.Products.Responses;

namespace Products.Application.Products.Queries;

public class GetAllProductsQuery : IRequest<IEnumerable<ProductResponse>> { }