using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Queries.LogsQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.LogsQueryHandlers
{
    public class GetLogByIdQueryHandler(ILogRepo logRepo)
    : IRequestHandler<GetLogByIdQuery, dtoLog>
    {
        private readonly ILogRepo _logRepo = logRepo;

        public async Task<dtoLog> Handle(GetLogByIdQuery request, CancellationToken cancellationToken)
        {
            var log = await _logRepo.GetLogById(request.LogId);

            return new dtoLog
            {
                LogId = log.LogId,
                Action = log.Action,
                TimeStamp = log.TimeStamp,
                UserId = log.UserId
            };
        }
    }
}
