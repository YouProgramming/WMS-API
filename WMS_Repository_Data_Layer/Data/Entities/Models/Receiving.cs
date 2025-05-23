using System;
using System.Collections.Generic;

namespace WMS_Repository_Data_Layer.Data.Entities.Models;

public partial class Receiving
{
    public int ReceiveId { get; set; }

    public int QuantityReceived { get; set; }

    public DateOnly ReceiveDate { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
