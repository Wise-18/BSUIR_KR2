using System.Text;
using System.Text.Json;
using BSUIR_KR1.API.Services;
using BSUIR_KR1.Domain.Entities;
using BSUIR_KR1.Domain.Models;

namespace BSUIR_KR1.UI.Services;

public class ApiProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiProductService> _logger;

    public ApiProductService(HttpClient httpClient, ILogger<ApiProductService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ResponseData<Product>> CreateProductAsync(Product product)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("products", product, _serializerOptions);
            if (!response.IsSuccessStatusCode)
            {
                return ResponseData<Product>.Error($"Error: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions)
                ?? ResponseData<Product>.Error("Unable to create product");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return ResponseData<Product>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile file)
    {
        try
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

            var response = await _httpClient.PostAsync($"products/{id}/image", content);
            if (!response.IsSuccessStatusCode)
            {
                return ResponseData<string>.Error($"Error uploading image: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<ResponseData<string>>(_serializerOptions)
                ?? ResponseData<string>.Error("Unable to upload image");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image");
            return ResponseData<string>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<bool>> DeleteProductAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return ResponseData<bool>.Error($"Error: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<ResponseData<bool>>(_serializerOptions)
                ?? ResponseData<bool>.Error("Unable to delete product");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product");
            return ResponseData<bool>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return ResponseData<Product>.Error($"Error: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions)
                ?? ResponseData<Product>.Error("Unable to get product");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product");
            return ResponseData<Product>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        try
        {
            var urlBuilder = new StringBuilder("products?");
            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                urlBuilder.Append($"category={Uri.EscapeDataString(categoryNormalizedName)}&");
            }
            urlBuilder.Append($"pageNo={pageNo}&pageSize={pageSize}");

            var response = await _httpClient.GetAsync(urlBuilder.ToString());
            if (!response.IsSuccessStatusCode)
            {
                return ResponseData<ListModel<Product>>.Error($"Error: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Product>>>(_serializerOptions)
                ?? ResponseData<ListModel<Product>>.Error("Unable to get products");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product list");
            return ResponseData<ListModel<Product>>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<bool>> UpdateProductAsync(int id, Product product)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"products/{id}", product, _serializerOptions);
            if (!response.IsSuccessStatusCode)
            {
                return ResponseData<bool>.Error($"Error: {response.StatusCode}");
            }

            return ResponseData<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product");
            return ResponseData<bool>.Error(ex.Message);
        }
    }
}