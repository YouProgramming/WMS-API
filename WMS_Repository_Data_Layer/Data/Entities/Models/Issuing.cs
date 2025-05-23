using System;
using System.Collections.Generic;

namespace WMS_Repository_Data_Layer.Data.Entities.Models;

public partial class Issuing
{
    public int IssueId { get; set; }

    public int QuantityIssued { get; set; }

    public DateOnly IssueDate { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
