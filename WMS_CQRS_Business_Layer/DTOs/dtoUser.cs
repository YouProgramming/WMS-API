using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoUser
    {
        [MaxLength(50)]
        public required string Username { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        public IFormFile? ProfilePicture { get; set; } = null;
        public string? ProfilePicturePath { get; set; } = null;

        [MaxLength(50)]
        public required string Email { get; set; }
    }
}
