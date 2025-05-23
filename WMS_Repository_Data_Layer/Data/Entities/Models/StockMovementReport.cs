using System;
using System.Collections.Generic;

namespace WMS_Repository_Data_Layer.Data.Entities.Models;

public partial class StockMovementReport
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
