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
    public class IssuingRepo(AppDbContext appDbContext) : IIssuingRepo
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<List<Issuing>> GetAllIssuings()
        {
            try
            {
                return await _appDbContext.Issuings.ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while retrieving issuings.", ex);
            }
        }
     
        public async Task<Issuing> GetIssuingById(int id)
        {
            try
            {
                var issuing = await _appDbContext.Issuings.SingleOrDefaultAsync(i => i.IssueId == id);

                return issuing ?? throw new KeyNotFoundException($"Issuing with ID {id} not found.");
            }
            catch (KeyNotFoundException)
            {
                throw;  
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while retrieving the issuing.", ex);
            }
        }

        public async Task<int> InsertIssuing(Issuing issuing)
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                await _appDbContext.Issuings.AddAsync(issuing);

                var product = await _appDbContext.Products
                    .SingleOrDefaultAsync(p => p.ProductId == issuing.ProductId) ?? throw new InvalidOperationException("Product not found. Please enter a valid product ID.");

                product.QuantityInStock -= issuing.QuantityIssued;

                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return issuing.IssueId;
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

                
                throw new ApplicationException("An error occurred while inserting the issuing.", ex);
            }
        }

        public async Task<int> UpdateIssuing(Issuing issuing)
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var existing = await _appDbContext.Issuings.FirstOrDefaultAsync(i => i.IssueId == issuing.IssueId) ??
                    throw new KeyNotFoundException($"Issuing with ID {issuing.IssueId} not found.");

                var prodduct = await _appDbContext.Products.FirstOrDefaultAsync(i => i.ProductId == issuing.ProductId) ??
                    throw new KeyNotFoundException($"Product with ID {issuing.ProductId} not found.");

                prodduct.QuantityInStock += existing.QuantityIssued;

                _appDbContext.Entry(existing).CurrentValues.SetValues(issuing);

                prodduct.QuantityInStock -= issuing.QuantityIssued;

                int RowsEffected = await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return RowsEffected;
               
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Transaction rollback failed.", ex);
                }
                throw;  
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the issuing.", ex);
            }
        }   
        
        public async Task<int> DeleteIssuing(int id)
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var existing = await _appDbContext.Issuings.FirstOrDefaultAsync(i => i.IssueId == id) ??
                   throw new KeyNotFoundException($"Issuing with ID {id} not found.");

                var prodduct = await _appDbContext.Products.FirstOrDefaultAsync(i => i.ProductId == existing.ProductId) ??
                    throw new KeyNotFoundException($"Product with ID {existing.ProductId} not found.");

                prodduct.QuantityInStock += existing.QuantityIssued;

                _appDbContext.Issuings.Remove(existing);

                int RowsEffected = await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return RowsEffected;
                
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred while deleting the issuing.", ex);
                }

                throw;  
            }
            catch (Exception ex)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception e)
                {
                    throw new ApplicationException("An error occurred while deleting the issuing.", e);
                }
                throw new ApplicationException("An error occurred while deleting the issuing.", ex);
            }
        }


    }

}