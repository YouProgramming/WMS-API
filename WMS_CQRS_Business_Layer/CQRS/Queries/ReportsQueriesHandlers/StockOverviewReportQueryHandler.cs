using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Queries.ReportsQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.ReportsQueriesHandlers
{
    public class StockOverviewReportQueryHandler(iStockOverviewReportRepo stockOverviewReportRepo) : IRequestHandler<StockOverviewReportQuery, List<dtoStockOverviewReport>>
    {
        private readonly iStockOverviewReportRepo _stockOverviewReportRepo = stockOverviewReportRepo;
        public async Task<List<dtoStockOverviewReport>> Handle(StockOverviewReportQuery request, CancellationToken cancellationToken)
        {
            var list = await _stockOverviewReportRepo.GetAllStockOverviewReports();

            return list.Select(s => new dtoStockOverviewReport
            {
                ProductId = s.ProductId,
                ProductName = s.ProductName,
                CategoryName = s.CategoryName,
                QuantityInStock = s.QuantityInStock,
                UnitPrice = s.UnitPrice,
                TotalPrice = s.TotalPrice,
                LastRestocked = s.LastRestocked
            }).ToList();
        }
    }
}
