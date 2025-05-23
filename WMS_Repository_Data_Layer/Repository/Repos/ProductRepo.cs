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
    public class ProductRepo(AppDbContext appDbContext) : IProductRepo
    {
        
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<List<Product>> GetAllProducts()
        {
            return await _appDbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _appDbContext.Products.SingleOrDefaultAsync(p => p.ProductId == id);

            return product is null ? throw new KeyNotFoundException($"Product with ID {id} was not found.") : product;
        }

        public async Task<Product> GetProductByName(string name)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.ProductName == name);

            return product is null ? throw new KeyNotFoundException($"Product with name '{name}' was not found.") : product;
        }

        public async Task<int> InsertProduct(Product product)
        {
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task<int> UpdateProduct(Product product)
        {
            var existingProduct = await _appDbContext.Products.FindAsync(product.ProductId)
                ?? throw new KeyNotFoundException($"Product with ID {product.ProductId} was not found.");

            _appDbContext.Entry(existingProduct).CurrentValues.SetValues(product);
            int result = await _appDbContext.SaveChangesAsync();
            return result;
        }

        public async Task<int> DeleteProduct(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id)
                ?? throw new KeyNotFoundException($"Product with ID {id} was not found.");

            _appDbContext.Products.Remove(product);
            int result = await _appDbContext.SaveChangesAsync();
            return result;
        }

        public async Task<bool> SaveProductPicturePath(string RelativePath, int ProductId)
        {
            var product = await _appDbContext.Products.SingleOrDefaultAsync(p => p.ProductId == ProductId);

            if (product == null)
                throw new KeyNotFoundException($"No product with ID '{ProductId}' found.");

            product.ProductImagePath = RelativePath;

            int result = await _appDbContext.SaveChangesAsync();

            return result > 0;
        }


        //public async Task<List<Product>> GetAllProducts()
        //{
        //    List<Product> products = [];

        //    try
        //    {
        //        products = await _appDbContext.Products.ToListAsync();

        //    }
        //    catch (Exception ex) 
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return products;
        //}

        //public async Task<Product> GetProductById(int id)
        //{
        //    Product? product;
        //    try
        //    {
        //        product = await _appDbContext.Products.SingleOrDefaultAsync(p => p.ProductId == id);

        //        if (product == null)
        //            throw new Exception("Not Found");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return product;
        //}

        //public async Task<Product> GetProductByName(string name)
        //{
        //    Product? product;
        //    try
        //    {
        //        product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.ProductName == name);
        //        if (product == null)
        //            throw new Exception("Not Found");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return product;
        //}

        //public async Task<int> InsertProduct(Product product)
        //{
        //    int ID = -1;
        //    try
        //    {

        //        await _appDbContext.Products.AddAsync(product);

        //        await _appDbContext.SaveChangesAsync();

        //        ID = product.ProductId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return ID;
        //}

        //public async Task<int> UpdateProduct(Product product)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        var Product = await _appDbContext.Products.FindAsync(product.ProductId) ?? throw new Exception("Not Found");


        //        _appDbContext.Entry(Product).CurrentValues.SetValues(product);

        //        Result = await _appDbContext.SaveChangesAsync(); ;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return Result;
        //}

        //public async Task<int> DeleteProduct(int id)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        var Product = await _appDbContext.Products.FindAsync(id) ?? throw new Exception("Not Found");

        //        _appDbContext.Products.Remove(Product);

        //        Result = await _appDbContext.SaveChangesAsync(); ;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return Result;
        //}
    }
}
