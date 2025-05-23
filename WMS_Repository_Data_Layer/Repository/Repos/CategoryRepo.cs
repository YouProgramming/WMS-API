using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WMS_Repository_Data_Layer.Data;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_Repository_Data_Layer.Repository.Repos
{
    public class CategoryRepo(AppDbContext appDbContext) : ICategoryRepo
    {
        private readonly AppDbContext _appDbContext = appDbContext;
       
        public async Task<List<Category>> GetAllCategories()
        {
            return await _appDbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _appDbContext.Categories
                .SingleOrDefaultAsync(c => c.CategoryId == id);

            return category is null ? throw new KeyNotFoundException($"Category with ID {id} not found.") : category;
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            var category = await _appDbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == name);

            return category is null ? throw new KeyNotFoundException($"Category with name '{name}' not found.") : category;
        }

        public async Task<int> InsertCategory(Category category)
        {
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return category.CategoryId;
        }

        public async Task<int> UpdateCategory(Category category)
        {
            var existingCategory = await _appDbContext.Categories
                .FindAsync(category.CategoryId)
                ?? throw new KeyNotFoundException($"Category with ID {category.CategoryId} not found.");

            _appDbContext.Entry(existingCategory).CurrentValues.SetValues(category);
            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteCategory(int id)
        {
            var category = await _appDbContext.Categories
                .FindAsync(id)
                ?? throw new KeyNotFoundException($"Category with ID {id} not found.");

            _appDbContext.Categories.Remove(category);
            return await _appDbContext.SaveChangesAsync();
        }

        //public async Task<List<Category>> GetAllCategories()
        //{
        //    List<Category> categories = [];

        //    try
        //    {
        //        categories = await _appDbContext.Categories.ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return categories;
        //}

        //public async Task<Category> GetCategoryById(int id)
        //{
        //    Category? category;
        //    try
        //    {
        //        category = await _appDbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == id);
        //        if (category == null)
        //            throw new Exception("Not Found");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return category;
        //}

        //public async Task<Category> GetCategoryByName(string name)
        //{
        //    Category? category;
        //    try
        //    {
        //        category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);
        //        if (category == null)
        //            throw new Exception("Not Found");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return category;
        //}

        //public async Task<int> InsertCategory(Category category)
        //{
        //    int ID = -1;
        //    try
        //    {
        //        await _appDbContext.Categories.AddAsync(category);
        //        await _appDbContext.SaveChangesAsync();
        //        ID = category.CategoryId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return ID;
        //}

        //public async Task<int> UpdateCategory(Category category)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        var existingCategory = await _appDbContext.Categories.FindAsync(category.CategoryId)
        //            ?? throw new Exception("Not Found");

        //        _appDbContext.Entry(existingCategory).CurrentValues.SetValues(category);
        //        Result = await _appDbContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return Result;
        //}

        //public async Task<int> DeleteCategory(int id)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        var category = await _appDbContext.Categories.FindAsync(id) ?? throw new Exception("Not Fount");

        //        _appDbContext.Categories.Remove(category);
        //        Result = await _appDbContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return Result;
        //}
    }
}
