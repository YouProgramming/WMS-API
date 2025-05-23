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
    public class GetAllUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUserQuery, List<dtoUser>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<List<dtoUser>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsers();

            if (users == null || !users.Any())
                return new List<dtoUser>();

           
            return users.Select(u => new dtoUser
            {
                Username = u.UserName,
                Name = u.Name,
                Email = u.Email,
                ProfilePicturePath = u.ProfilePicturePath,
            }).ToList();

        }
    }
}
