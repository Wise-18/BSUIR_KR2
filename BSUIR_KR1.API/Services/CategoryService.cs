﻿using BSUIR_KR1.API.Data;
using BSUIR_KR1.Domain.Entities;
using BSUIR_KR1.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BSUIR_KR1.API.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        try
        {
            var categories = await _context.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(categories);
        }
        catch (Exception ex)
        {
            return ResponseData<List<Category>>.Error(ex.Message);
        }
    }
}