using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.UserCommands
{
    public class RegisterUserCommandHandler(IUserRepository userRepository) : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser user = new()
            {
                UserName = request.RegisterUser.Username,
                Email = request.RegisterUser.Email,
                Name = request.RegisterUser.Name,
            };

            return await _userRepository.RegisterAsync(user, request.RegisterUser.Password);
        }
    }
}
