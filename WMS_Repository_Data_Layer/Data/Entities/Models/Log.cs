using System;
using System.Collections.Generic;

namespace WMS_Repository_Data_Layer.Data.Entities.Models;

public partial class Log
{
    public int LogId { get; set; }

    public string Action { get; set; } = null!;

    public DateTime TimeStamp { get; set; }

    public required string UserId { get; set; }

    public virtual ApplicationUser User { get; set; } = null!;
}
