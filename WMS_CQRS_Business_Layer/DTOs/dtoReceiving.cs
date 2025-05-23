using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoReceiving
    {
        public int ReceiveId { get; set; }
        public int QuantityReceived { get; set; }
        public DateOnly ReceiveDate { get; set; }
        public int ProductId { get; set; }

        public bool Validate()
        {
            return QuantityReceived > 0 && ProductId > 0;
        }
    }

}
