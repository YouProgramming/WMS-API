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
    public class StockMovementReportRepo(AppDbContext appDbContext) : IStockMovementReportRepo
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<List<StockMovementReport>> GetAllStockMovementReports()
        {
            try
            {
                return await _appDbContext.StockMovementReports.ToListAsync();

            }
            catch(Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving Reports.", ex);
            }
        }
    }
}
