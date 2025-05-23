using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.UserCommands
{
    public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.DeleteUserAsync(request.Username);
        }
    }
}
