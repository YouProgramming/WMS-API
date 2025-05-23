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
    public class GetAllLogsQueryHandler(ILogRepo logRepo)
    : IRequestHandler<GetAllLogsQuery, List<dtoLog>>
    {
        private readonly ILogRepo _logRepo = logRepo;

        public async Task<List<dtoLog>> Handle(GetAllLogsQuery request, CancellationToken cancellationToken)
        {
            var list = await _logRepo.GetAllLogs();

            return list.Select(l => new dtoLog
            {
                LogId = l.LogId,
                Action = l.Action,
                TimeStamp = l.TimeStamp,
                UserId = l.UserId
            }).ToList();
        }
    }
}
