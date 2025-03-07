using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Products.Commands;
using Products.Application.Products.Queries;

namespace Products.Api.Controllers.v1;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <returns>The product if found; otherwise, 404 Not Found.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        _logger.LogInformation("Fetching product with ID {ProductId}", id);

        var product = await _mediator.Send(new GetProductByIdQuery { Id = id });

        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
            return NotFound(new { Message = $"Product with ID {id} not found" });
        }

        return Ok(product);
    }
    
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="command">The product creation request.</param>
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid product creation request: {@Command}", command);
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new product: {@Command}", command);

        var productId = await _mediator.Send(command);
        var product = await _mediator.Send(new GetProductByIdQuery { Id = productId });

        return CreatedAtAction(nameof(GetProductById), new { id = productId }, product);
    }
}
