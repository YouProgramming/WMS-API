using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_Repository_Data_Layer.Repository.IRepos
{
    public interface IStockMovementReportRepo
    {
        public Task<List<StockMovementReport>> GetAllStockMovementReports();
    }
}
