using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class ProductsController : BaseApiController
{
    private readonly IGenericRepository<Product> _productRepository;

    public ProductsController(IGenericRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);
        
        return await CreatePagedResult(_productRepository, spec, specParams.PageIndex, specParams.PageSize);
    } 

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if(product is null)
             return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        await _productRepository.AddAsync(product);
        if(await _productRepository.SaveChangesAsync())
            return CreatedAtAction(nameof(GetProduct), new {id = product.Id}, product);

        return BadRequest("Failed to create the product!");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if(id != product.Id || !ProductExists(id))
            return BadRequest("Cannot update product with the given id!");

        _productRepository.Update(product);
       if( await _productRepository.SaveChangesAsync())
            return NoContent();

        return BadRequest("Failed to update the product!");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if(product is null)
            return NotFound();

        _productRepository.Delete(product);
        if(await _productRepository.SaveChangesAsync())
            return NoContent();

        return BadRequest("Failed to delete the product!");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        var brands = await _productRepository.ListAsync(spec);
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        var types = await _productRepository.ListAsync(spec);
        return Ok(types);
    }

    private bool ProductExists(int id)
    {
        return _productRepository.Exists(id);
    }
}
