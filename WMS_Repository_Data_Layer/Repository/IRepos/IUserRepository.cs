using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_Repository_Data_Layer.Repository.IRepos
{
    public interface IUserRepository
    {
        public Task<IdentityResult> RegisterAsync(ApplicationUser User, string Password);
        public Task<string> LoginAsync(string username, string password);
        public Task<bool> DeleteUserAsync(string Username);
        public Task<List<ApplicationUser>> GetAllUsers();
        public Task<bool> SavePfpRelativePath(string Relative, string Username);
        public Task<ApplicationUser> GetUserByUsername(string Username);
        // Adds a method to update an existing user asynchronously
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user, string NewPassword);
    }
}
