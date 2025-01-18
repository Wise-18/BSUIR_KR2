using BSUIR_KR1.API.Data;
using BSUIR_KR1.Domain.Entities;
using BSUIR_KR1.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BSUIR_KR1.API.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly int _maxPageSize = 20;

    public ProductService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<ResponseData<Product>> CreateProductAsync(Product product)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return ResponseData<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return ResponseData<Product>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<bool>> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return ResponseData<bool>.Error("Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return ResponseData<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ResponseData<bool>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return ResponseData<Product>.Error("Product not found");

            return ResponseData<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return ResponseData<Product>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > _maxPageSize)
            pageSize = _maxPageSize;

        try
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                query = query.Where(p => p.Category != null &&
                    p.Category.NormalizedName == categoryNormalizedName);
            }

            var count = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            pageNo = Math.Max(1, Math.Min(pageNo, totalPages));

            var items = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return ResponseData<ListModel<Product>>.Success(new ListModel<Product>
            {
                Items = items,
                CurrentPage = pageNo,
                TotalPages = totalPages
            });
        }
        catch (Exception ex)
        {
            return ResponseData<ListModel<Product>>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return ResponseData<string>.Error("Product not found");

            var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
            var filePath = Path.Combine(imageFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            product.Image = $"/Images/{fileName}";
            product.MimeType = formFile.ContentType;
            await _context.SaveChangesAsync();

            return ResponseData<string>.Success(product.Image);
        }
        catch (Exception ex)
        {
            return ResponseData<string>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<bool>> UpdateProductAsync(int id, Product product)
    {
        try
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return ResponseData<bool>.Error("Product not found");

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();
            return ResponseData<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ResponseData<bool>.Error(ex.Message);
        }
    }
}