using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoStockOverviewReport
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public int? QuantityInStock { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? TotalPrice { get; set; }

        public DateOnly? LastRestocked { get; set; }
    }
}
