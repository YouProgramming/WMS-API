using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.UserCommands
{
    public record UpdateUserPfpCommand(string RelativePath, string UserName) : IRequest<bool>;
}
