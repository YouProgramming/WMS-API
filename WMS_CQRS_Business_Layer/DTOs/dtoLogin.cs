using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoLogin
    {
        public required string Username { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;

        public bool IsValid()
        {
            return Password != string.Empty && Username != string.Empty;
        }
    }
}
