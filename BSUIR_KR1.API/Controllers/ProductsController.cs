using Microsoft.AspNetCore.Mvc;
using BSUIR_KR1.API.Services;
using BSUIR_KR1.Domain.Entities;
using BSUIR_KR1.Domain.Models;

namespace BSUIR_KR1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseData<ListModel<Product>>>> GetProducts(
        [FromQuery] string? category,
        [FromQuery] int pageNo = 1,
        [FromQuery] int pageSize = 3)
    {
        var response = await _productService.GetProductListAsync(category, pageNo, pageSize);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseData<Product>>> GetProduct(int id)
    {
        var response = await _productService.GetProductByIdAsync(id);
        if (!response.Successfull)
            return NotFound(response);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ResponseData<Product>>> CreateProduct([FromBody] Product product)
    {
        var response = await _productService.CreateProductAsync(product);
        if (!response.Successfull)
            return BadRequest(response);
        return CreatedAtAction(nameof(GetProduct), new { id = response.Data.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseData<bool>>> UpdateProduct(int id, [FromBody] Product product)
    {
        var response = await _productService.UpdateProductAsync(id, product);
        if (!response.Successfull)
            return NotFound(response);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseData<bool>>> DeleteProduct(int id)
    {
        var response = await _productService.DeleteProductAsync(id);
        if (!response.Successfull)
            return NotFound(response);
        return Ok(response);
    }

    [HttpPost("{id}/image")]
    public async Task<ActionResult<ResponseData<string>>> UploadImage(int id, IFormFile file)
    {
        var response = await _productService.SaveImageAsync(id, file);
        if (!response.Successfull)
            return BadRequest(response);
        return Ok(response);
    }
}