using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.UserCommands
{
    internal class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser user = new()
            {
                UserName = request.User.Username,
                Email = request.User.Email,
                Name = request.User.Name,
            };
           
            var result = await _userRepository.UpdateUserAsync(user, request.User.NewPassword);
            

            return result.Succeeded;
        }
    }
}
