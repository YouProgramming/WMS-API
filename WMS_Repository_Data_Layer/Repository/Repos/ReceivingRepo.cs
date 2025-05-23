using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Data;
using WMS_Repository_Data_Layer.Repository.IRepos;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WMS_Repository_Data_Layer.Repository.Repos
{
    public class ReceivingRepo(AppDbContext appDbContext) : IReceivingRepo
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<List<Receiving>> GetAllReceivings()
        {
            return await _appDbContext.Receivings.ToListAsync();
        }

        public async Task<Receiving> GetReceivingById(int id)
        {
            var receiving = await _appDbContext.Receivings
                .SingleOrDefaultAsync(r => r.ReceiveId == id);

            return receiving ?? throw new KeyNotFoundException($"Receiving with ID {id} not found.");
        }

        public async Task<int> InsertReceiving(Receiving receiving)
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                await _appDbContext.Receivings.AddAsync(receiving);

                var product = await _appDbContext.Products
                    .SingleOrDefaultAsync(p => p.ProductId == receiving.ProductId)
                    ?? throw new InvalidOperationException("Product not found. Please enter a valid product ID.");

                product.QuantityInStock += receiving.QuantityReceived;

                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return receiving.ReceiveId;
            }
            catch (Exception ex)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception rollbackEx)
                {
                    throw new ApplicationException("Transaction rollback failed.", rollbackEx);
                }

                throw new ApplicationException("An error occurred while inserting the receiving.", ex);
            }
        }

        public async Task<int> UpdateReceiving(Receiving receiving)
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                var existing = await _appDbContext.Receivings
                    .FirstOrDefaultAsync(r => r.ReceiveId == receiving.ReceiveId)
                    ?? throw new KeyNotFoundException($"Receiving with ID {receiving.ReceiveId} not found.");

                var product = await _appDbContext.Products
                    .FirstOrDefaultAsync(p => p.ProductId == receiving.ProductId)
                    ?? throw new KeyNotFoundException($"Product with ID {receiving.ProductId} not found.");

                // Adjust stock quantity before update
                product.QuantityInStock -= existing.QuantityReceived;

                _appDbContext.Entry(existing).CurrentValues.SetValues(receiving);

                product.QuantityInStock += receiving.QuantityReceived;

                int rowsAffected = await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return rowsAffected;
            }
            catch (Exception ex)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception rollbackEx)
                {
                    throw new ApplicationException("Transaction rollback failed.", rollbackEx);
                }

                throw new ApplicationException("An error occurred while updating the receiving.", ex);
            }
        }

        public async Task<int> DeleteReceiving(int id)
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                var existing = await _appDbContext.Receivings
                    .FirstOrDefaultAsync(r => r.ReceiveId == id)
                    ?? throw new KeyNotFoundException($"Receiving with ID {id} not found.");

                var product = await _appDbContext.Products
                    .FirstOrDefaultAsync(p => p.ProductId == existing.ProductId)
                    ?? throw new KeyNotFoundException($"Product with ID {existing.ProductId} not found.");

                product.QuantityInStock -= existing.QuantityReceived;

                _appDbContext.Receivings.Remove(existing);

                int rowsAffected = await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return rowsAffected;
            }
            catch (Exception ex)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception rollbackEx)
                {
                    throw new ApplicationException("Transaction rollback failed.", rollbackEx);
                }

                throw new ApplicationException("An error occurred while deleting the receiving.", ex);
            }
        }


        //public async Task<List<Receiving>> GetAllReceivings()
        //{
        //    try
        //    {
        //        return await _appDbContext.Receivings.ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException("An error occurred while retrieving receivings.", ex);
        //    }
        //}

        //public async Task<Receiving> GetReceivingById(int id)
        //{
        //    try
        //    {
        //        var receiving = await _appDbContext.Receivings
        //            .SingleOrDefaultAsync(r => r.ReceiveId == id);

        //        return receiving ?? throw new KeyNotFoundException($"Receiving with ID {id} not found.");
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException("An error occurred while retrieving the receiving.", ex);
        //    }
        //}

        //public async Task<int> InsertReceiving(Receiving receiving)
        //{
        //    await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        await _appDbContext.Receivings.AddAsync(receiving);

        //        var product = await _appDbContext.Products
        //           .SingleOrDefaultAsync(p => p.ProductId == receiving.ProductId) ?? throw new InvalidOperationException("Product not found. Please enter a valid product ID.");

        //        product.QuantityInStock += receiving.QuantityReceived;

        //        await _appDbContext.SaveChangesAsync();

        //        await transaction.CommitAsync();
        //        return receiving.ReceiveId;
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            await transaction.RollbackAsync();
        //        }
        //        catch (Exception rollbackEx)
        //        {
        //            throw new ApplicationException("Transaction rollback failed.", rollbackEx);
        //        }

        //        throw new ApplicationException("An error occurred while inserting the receiving.", ex);
        //    }
        //}

        //public async Task<int> UpdateReceiving(Receiving receiving)
        //{
        //    await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        var existing = await _appDbContext.Receivings
        //            .FirstOrDefaultAsync(r => r.ReceiveId == receiving.ReceiveId)
        //            ?? throw new KeyNotFoundException($"Receiving with ID {receiving.ReceiveId} not found.");

        //        var prodduct = await _appDbContext.Products.FirstOrDefaultAsync(i => i.ProductId == receiving.ProductId) ??
        //          throw new KeyNotFoundException($"Product with ID {receiving.ProductId} not found.");

        //        prodduct.QuantityInStock -= existing.QuantityReceived;

        //        _appDbContext.Entry(existing).CurrentValues.SetValues(receiving);

        //        prodduct.QuantityInStock += existing.QuantityReceived;

        //        int rowsAffected = await _appDbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return rowsAffected;
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        try
        //        {
        //            await transaction.RollbackAsync();
        //        }
        //        catch (Exception rollbackEx)
        //        {
        //            throw new ApplicationException("Transaction rollback failed.", rollbackEx);
        //        }

        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            await transaction.RollbackAsync();
        //        }
        //        catch (Exception rollbackEx)
        //        {
        //            throw new ApplicationException("Transaction rollback failed.", rollbackEx);
        //        }

        //        throw new ApplicationException("An error occurred while updating the receiving.", ex);
        //    }
        //}
        //public async Task<int> DeleteReceiving(int id)
        //{
        //    await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        var existing = await _appDbContext.Receivings
        //            .FirstOrDefaultAsync(r => r.ReceiveId == id)
        //            ?? throw new KeyNotFoundException($"Receiving with ID {id} not found.");

        //        var prodduct = await _appDbContext.Products.FirstOrDefaultAsync(i => i.ProductId == existing.ProductId) ??
        //           throw new KeyNotFoundException($"Product with ID {existing.ProductId} not found.");

        //        prodduct.QuantityInStock -= existing.QuantityReceived;

        //        _appDbContext.Receivings.Remove(existing);

        //        int rowsAffected = await _appDbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return rowsAffected;
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        try
        //        {
        //            await transaction.RollbackAsync();
        //        }
        //        catch (Exception rollbackEx)
        //        {
        //            throw new ApplicationException("Transaction rollback failed.", rollbackEx);
        //        }

        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            await transaction.RollbackAsync();
        //        }
        //        catch (Exception rollbackEx)
        //        {
        //            throw new ApplicationException("Transaction rollback failed.", rollbackEx);
        //        }

        //        throw new ApplicationException("An error occurred while deleting the receiving.", ex);
        //    }
        //}
    }

}
