using System;
using System.Collections.Generic;

namespace WMS_Repository_Data_Layer.Data.Entities.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? ProductImagePath { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int QuantityInStock { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Issuing> Issuings { get; set; } = new List<Issuing>();

    public virtual ICollection<Receiving> Receivings { get; set; } = new List<Receiving>();
}
