using Microsoft.AspNetCore.Mvc;
using BSUIR_KR1.API.Services;
using BSUIR_KR1.Domain.Entities;
using BSUIR_KR1.Domain.Models;

namespace BSUIR_KR1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
    {
        var response = await _categoryService.GetCategoryListAsync();
        return Ok(response);
    }
}