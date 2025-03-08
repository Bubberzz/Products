using MediatR;
using Products.Application.Products.Queries;
using Products.Application.Products.Responses;
using Products.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Products.Domain.Exceptions;

namespace Products.Application.Products.Handlers;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(IProductRepository repository, ILogger<GetProductByIdQueryHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger;
    }

    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching product with ID: {Id}", request.Id);

        var product = await _repository.GetByIdAsync(request.Id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {Id} not found.", request.Id);
            throw new NotFoundException($"Product with ID {request.Id} not found.");
        }

        return new ProductResponse
        {
            Id = product.Id.Value,
            Name = product.Name,
            Price = product.Price.Value,
            Stock = product.Stock.Value
        };
    }
}