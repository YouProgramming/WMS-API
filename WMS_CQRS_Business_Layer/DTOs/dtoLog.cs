using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoLog
    {
        public int LogId { get; set; }
        public string Action { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
        public required string UserId { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(Action) && UserId != null;
        }
    }

}
