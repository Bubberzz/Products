using MediatR;
using Products.Application.Products.Commands;
using Products.Domain.Entities;
using Products.Domain.Events;
using Products.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Products.Domain.ValueObjects;

namespace Products.Application.Products.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductId>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(
        IProductRepository repository, 
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<CreateProductCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ProductId> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating product: Name: {Name}, Price: {Price}, Stock: {Stock}", 
            request.Name, request.Price, request.Stock);

        var product = new Product(
            ProductId.New(),
            request.Name,
            new Price(request.Price),
            new Stock(request.Stock)
        );
        await _repository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        await _mediator.Publish(new ProductCreatedEvent(product.Id, product.Name, product.Price, product.Stock), cancellationToken);

        _logger.LogInformation("Product created successfully: {ProductId}", product.Id.Value);
        return product.Id;
    }
}