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
    public class StockMovementReportQueryHandler(IStockMovementReportRepo stockMovementReportRepo) : IRequestHandler<StockMovementReportQuery, List<dtoStockMovementReport>>
    {
        private readonly IStockMovementReportRepo _stockMovementReportRepo = stockMovementReportRepo;
        public async Task<List<dtoStockMovementReport>> Handle(StockMovementReportQuery request, CancellationToken cancellationToken)
        {
            var list = await _stockMovementReportRepo.GetAllStockMovementReports();

            return list.Select( s => new dtoStockMovementReport
            {
                ProductId = s.ProductId,
                ProductName = s.ProductName,
                CategoryName = s.CategoryName,
                MovementDate = s.MovementDate,
                TransactionType = s.TransactionType,
                Quantity = s.Quantity,
                UnitPrice = s.UnitPrice,
                TotalValue = s.TotalValue
            }).ToList();
        }
    }
}
