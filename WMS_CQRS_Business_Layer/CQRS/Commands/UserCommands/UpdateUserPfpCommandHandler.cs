using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.UserCommands
{
    public class UpdateUserPfpCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserPfpCommand, bool>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<bool> Handle(UpdateUserPfpCommand request, CancellationToken cancellationToken)
        {
            if(await _userRepository.SavePfpRelativePath(request.RelativePath, request.UserName))
                return true;

            return false;
        }
    }
}
