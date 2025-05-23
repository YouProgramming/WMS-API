using System;
using System.Collections.Generic;

namespace WMS_Repository_Data_Layer.Data.Entities.Models;

public partial class StockOverviewReport
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public int? QuantityInStock { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateOnly? LastRestocked { get; set; }
}
