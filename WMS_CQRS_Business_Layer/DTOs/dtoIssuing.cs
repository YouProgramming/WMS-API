using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoIssuing
    {
        public int IssueId { get; set; }
        public int QuantityIssued { get; set; }
        public DateOnly IssueDate { get; set; }
        public int ProductId { get; set; }
        public bool Validate()
        {
            return QuantityIssued > 0 && ProductId > 0;
        }
    }
}
