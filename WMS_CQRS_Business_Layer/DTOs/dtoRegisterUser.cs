using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoRegisterUser
    {
        public required string Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? NewPassword {  get; set; } = null;
        public IFormFile? ProfilePicture { get; set; } = null;
        public required string Name {  get; set; } = string.Empty;  
        public required string Email { get; set; } = string.Empty;

        public bool Validate()
        {
            return Username != string.Empty && Password != string.Empty && Email != string.Empty && Name != string.Empty;
        }
    }
}
