using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WMS_Repository_Data_Layer.Data.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty!;
        public string? ProfilePicturePath { get; set; } = string.Empty!;
        public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
    }
}
