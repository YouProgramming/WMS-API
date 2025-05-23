using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.UserCommands
{
    public class LoginCommandHandler(IUserRepository userRepository) : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserRepository _userRepository = userRepository;


        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var Token = await _userRepository.LoginAsync(request.Login.Username, request.Login.Password);

            return Token.ToString();
        }
    }
}
