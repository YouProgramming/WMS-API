using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.DTOs;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.LogsQueries
{
    public record GetLogByIdQuery(int LogId) : IRequest<dtoLog>;
}
