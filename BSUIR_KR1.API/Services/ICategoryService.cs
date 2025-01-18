using BSUIR_KR1.Domain.Entities;
using BSUIR_KR1.Domain.Models;

namespace BSUIR_KR1.API.Services;

public interface ICategoryService
{
    Task<ResponseData<List<Category>>> GetCategoryListAsync();
}