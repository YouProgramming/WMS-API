using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.LogCommands;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.LogCommandsHandlers
{
    public class UpdateLogCommandHandler(ILogRepo logRepo)
    : IRequestHandler<UpdateLogCommand, bool>
    {
        private readonly ILogRepo _logRepo = logRepo;

        public async Task<bool> Handle(UpdateLogCommand request, CancellationToken cancellationToken)
        {
            Log log = new()
            {
                LogId = request.Log.LogId,
                Action = request.Log.Action,
                TimeStamp = request.Log.TimeStamp,
                UserId = request.Log.UserId
            };

            var result = await _logRepo.UpdateLog(log);
            return result > 0;
        }
    }
}
