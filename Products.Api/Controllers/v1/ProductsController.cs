using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Products.Commands;
using Products.Application.Products.Queries;
using Products.Domain.ValueObjects;

namespace Products.Api.Controllers.v1;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery { Id = new ProductId(id) });

        if (product == null)
            return NotFound(new { Message = $"Product with ID {id} not found" });

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var productId = await _mediator.Send(command);
        var product = await _mediator.Send(new GetProductByIdQuery { Id = productId });

        return CreatedAtAction(nameof(GetProductById), new { id = productId.Value }, product);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id.Value)
            return BadRequest(new { Message = "ID in request does not match route ID" });

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand { Id = new ProductId(id) });
        return NoContent();
    }
}
