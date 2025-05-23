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
    public class AddNewLogCommandHandler(ILogRepo logRepo)
    : IRequestHandler<AddNewLogCommand, int>
    {
        private readonly ILogRepo _logRepo = logRepo;

        public async Task<int> Handle(AddNewLogCommand request, CancellationToken cancellationToken)
        {
            Log log = new()
            {
                Action = request.Log.Action,
                TimeStamp = request.Log.TimeStamp,
                UserId = request.Log.UserId
            };

            return await _logRepo.InsertLog(log);
        }
    }
}
