using Microsoft.AspNetCore.Mvc;
using BSUIR_KR1.UI.Services;
using BSUIR_KR1.Domain.Models;
using BSUIR_KR1.API.Services;

namespace BSUIR_KR1.UI.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
        var productResponse = await _productService.GetProductListAsync(category, pageNo);
        if (!productResponse.Successfull)
        {
            return NotFound(productResponse.ErrorMessage);
        }

        var categoriesResponse = await _categoryService.GetCategoryListAsync();
        if (!categoriesResponse.Successfull)
        {
            return NotFound(categoriesResponse.ErrorMessage);
        }

        ViewData["Categories"] = categoriesResponse.Data;
        ViewData["CurrentCategory"] = string.IsNullOrEmpty(category) ? "Все" :
            categoriesResponse.Data?.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";

        return View(productResponse.Data);
    }
}