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


    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        _logger.LogInformation("Fetching all products");

        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }
    
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
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            _logger.LogWarning("Product update failed: ID in request does not match route ID.");
            return BadRequest(new { Message = "ID in request does not match route ID" });
        }

        _logger.LogInformation("Updating product with ID {ProductId}: {@Command}", id, command);

        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        _logger.LogInformation("Deleting product with ID {ProductId}", id);

        await _mediator.Send(new DeleteProductCommand { Id = id });
        return NoContent();
    }
}
