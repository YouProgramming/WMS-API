using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoStockMovementReport
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public DateOnly MovementDate { get; set; }

        public string TransactionType { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? TotalValue { get; set; }
    }
}
