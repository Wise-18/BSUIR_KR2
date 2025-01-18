using BSUIR_KR1.Domain.Entities;
using BSUIR_KR1.Domain.Models;

namespace BSUIR_KR1.API.Services;

public interface IProductService
{
    Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
    Task<ResponseData<Product>> GetProductByIdAsync(int id);
    Task<ResponseData<Product>> CreateProductAsync(Product product);
    Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    Task<ResponseData<bool>> UpdateProductAsync(int id, Product product);
    Task<ResponseData<bool>> DeleteProductAsync(int id);
}