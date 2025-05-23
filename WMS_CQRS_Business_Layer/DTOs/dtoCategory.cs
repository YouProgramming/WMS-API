using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoCategory
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public bool isValid()
        {
            return !(string.IsNullOrEmpty(CategoryName));
        }
    }
}
