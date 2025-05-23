using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.LogCommands;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.LogCommandsHandlers
{
    public class DeleteLogCommandHandler(ILogRepo logRepo)
    : IRequestHandler<DeleteLogCommand, bool>
    {
        private readonly ILogRepo _logRepo = logRepo;

        public async Task<bool> Handle(DeleteLogCommand request, CancellationToken cancellationToken)
        {
            var result = await _logRepo.DeleteLog(request.LogId);
            return result > 0;
        }
    }
}
