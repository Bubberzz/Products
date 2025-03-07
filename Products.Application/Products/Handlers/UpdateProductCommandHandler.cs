using MediatR;
using Microsoft.Extensions.Logging;
using Products.Application.Products.Commands;
using Products.Domain.Events;
using Products.Domain.Exceptions;
using Products.Domain.Interfaces;
using Products.Domain.ValueObjects;

namespace Products.Application.Products.Handlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateProductCommandHandler> _logger;
    
    public UpdateProductCommandHandler(
        IProductRepository repository, 
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<UpdateProductCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);
        if (product == null)
            throw new NotFoundException($"Product with ID {request.Id} not found."); // ✅ Throw NotFoundException

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Product name cannot be empty.");
        
        product.UpdateDetails(request.Name, new Price(request.Price), new Stock(request.Stock));

        await _repository.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(); 
        
        await _mediator.Publish(
            new ProductUpdatedEvent(
                product.Id, 
                product.Name, 
                product.Price.Value,
                product.Stock.Value 
            ), cancellationToken);
    }
}