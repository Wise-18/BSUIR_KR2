using BSUIR_KR1.API.Services;
using BSUIR_KR1.Domain.Entities;
using BSUIR_KR1.Domain.Models;
using BSUIR_KR1.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BSUIR_KR1.UI.Areas.Admin.Pages;

public class IndexModel : PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    public ListModel<Product> Products { get; set; } = new();

    public async Task OnGetAsync(int pageNo = 1)
    {
        var response = await _productService.GetProductListAsync(null, pageNo);
        if (response.Successfull)
        {
            Products = response.Data;
        }
    }
}