using MediatR;
using Microsoft.Extensions.Logging;
using Products.Application.Products.Commands;
using Products.Domain.Events;
using Products.Domain.Exceptions;
using Products.Domain.Interfaces;

namespace Products.Application.Products.Handlers;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(
        IProductRepository repository, 
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<DeleteProductCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", request.Id);

        var product = await _repository.GetByIdAsync(request.Id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found.", request.Id);
            throw new NotFoundException($"Product with ID {request.Id} not found.");
        }

        await _repository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        await _mediator.Publish(new ProductDeletedEvent(product.Id), cancellationToken);

        _logger.LogInformation("Product with ID {ProductId} deleted successfully.", request.Id);
    }
}