using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.IssuingCommands
{
    public record DeleteIssuingCommand(int IssueId) : IRequest<bool>;
}
