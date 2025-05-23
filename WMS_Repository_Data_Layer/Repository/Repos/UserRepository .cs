using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WMS_Repository_Data_Layer.Data;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_Repository_Data_Layer.Repository.Repos
{
    public class UserRepository(UserManager<ApplicationUser> userManager, IConfiguration configuration) : IUserRepository
    {
        private readonly IConfiguration configuration = configuration;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        //private async Task<string> GenerateToken(ApplicationUser user)
        //{
        //    var Claims = new List<Claim>();

        //    if (string.IsNullOrEmpty(user.UserName))
        //        throw new ArgumentException("Username is not valid");

        //    Claims.Add(new Claim(ClaimTypes.Name, user.UserName));
        //    Claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        //    Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        //    var roles = await _userManager.GetRolesAsync(user);

        //    foreach (var role in roles)
        //    {
        //        Claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        //    }

        //    string SecretKey = configuration["JWT:SecretKey"] ?? throw new Exception("Secret Key Not Found");

        //    var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        //    var sc = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //            claims: Claims,
        //            issuer: configuration["JWT:Issuer"],
        //            audience: configuration["JWT:Audience"],
        //            expires: DateTime.Now.AddMinutes(1),
        //            signingCredentials: sc
        //            );


        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            
            if (string.IsNullOrEmpty(user.UserName))
                throw new ArgumentException("Username is not valid");

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("Secret Key not found in configuration.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //public async Task<string> LoginAsync(string username, string password)
        //{
        //    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        //        throw new ArgumentException("Username or password is not valid");

        //    var user = await _userManager.FindByNameAsync(username) ?? throw new Exception("User not found");
        //    var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        //    if (!isPasswordValid)
        //        throw new ArgumentException("Password is not valid");


        //    return await GenerateToken(user);
        //}

        public async Task<string> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username or password is not valid");

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid password");

            string token = await GenerateToken(user);
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("Failed to generate authentication token.");
            }
            return token;
        }

        //public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
        //{
        //    if (user == null || string.IsNullOrWhiteSpace(password))
        //        throw new ArgumentException("User or password is not valid");

        //    var newUser = new ApplicationUser
        //    {
        //        UserName = user.UserName,
        //        Email = user.Email,
        //        Name = user.Name 
        //    };

        //    return await _userManager.CreateAsync(newUser, password);
        //}

        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
        {
            if (user == null || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("User or password is not valid");

            var newUser = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name
            };

            
            return await _userManager.CreateAsync(newUser, password);
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByUsername(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username);
            if (user == null)
                throw new KeyNotFoundException($"No user with username '{Username}' found");
               
            return user;
        }

        public async Task<bool> SavePfpRelativePath(string Relative, string Username)
        {
            var user = await _userManager.FindByNameAsync(Username);
            if(user == null)
                throw new KeyNotFoundException($"No user with username '{Username}' found");

            user.ProfilePicturePath = Relative;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return true;


            return false;
        }

        public async Task<bool> DeleteUserAsync(string Username)
        {
            var product = await _userManager.FindByNameAsync(Username)
                ?? throw new KeyNotFoundException($"User with UserName {Username} was not found.");

            var result = await _userManager.DeleteAsync(product);
            return result.Succeeded;
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user, string? newPassword)
        {
            if (user == null || user.UserName == null)
                throw new ArgumentNullException(nameof(user));

            // Find the existing user by username
            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with Id '{user.Id}' not found.");

            // Update basic properties
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Name = user.Name;

            // Update the user
            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
                return updateResult;

            if (newPassword != null && newPassword != "" && newPassword != "null")
            {
                // Remove existing password if there is one
                var hasPassword = await _userManager.HasPasswordAsync(existingUser);
                if (hasPassword)
                {
                    var removeResult = await _userManager.RemovePasswordAsync(existingUser);
                    if (!removeResult.Succeeded)
                        return removeResult;
                }

                // Add new password
                var addResult = await _userManager.AddPasswordAsync(existingUser, newPassword);
                return addResult;
            }

            return IdentityResult.Success;
        }

    }
}
