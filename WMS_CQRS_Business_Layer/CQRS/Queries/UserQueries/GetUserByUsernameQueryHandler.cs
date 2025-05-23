using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.UserQueries
{
    internal class GetUserByUsernameQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByUsernameQuery, dtoUser>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<dtoUser> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsername(request.Username);

            if (user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.UserName))
                throw new UnauthorizedAccessException("Username / password not valid");

            return new dtoUser
            {
                Email = user.Email,
                Name = user.Name,
                Username = user.UserName,
                ProfilePicturePath = user.ProfilePicturePath,
            };
        }
    }
}
