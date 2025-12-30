using Application.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using API.Responses;

namespace API.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        var response = products.Adapt<IReadOnlyList<ProductResponse>>();
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        return Ok(product.Adapt<ProductResponse>());
    }
}
