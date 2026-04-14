using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts
    (string? brand = null, string? type = null, string? sort = null)
    {
        return Ok(await _productRepository.GetProductsAsync(brand, type, sort));
    } 

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if(product is null)
             return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _productRepository.AddProduct(product);
        if(await _productRepository.SaveChangesAsync())
            return CreatedAtAction(nameof(GetProduct), new {id = product.Id}, product);

        return BadRequest("Failed to create the product!");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if(id != product.Id || !ProductExists(id))
            return BadRequest("Cannot update product with the given id!");

        _productRepository.UpdateProduct(product);
       if( await _productRepository.SaveChangesAsync())
            return NoContent();

        return BadRequest("Failed to update the product!");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if(product is null)
            return NotFound();

        _productRepository.DeleteProduct(product);
        if(await _productRepository.SaveChangesAsync())
            return NoContent();

        return BadRequest("Failed to delete the product!");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await _productRepository.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await _productRepository.GetTypesAsync());
    }

    private bool ProductExists(int id)
    {
        return _productRepository.ProductExists(id);
    }
}
