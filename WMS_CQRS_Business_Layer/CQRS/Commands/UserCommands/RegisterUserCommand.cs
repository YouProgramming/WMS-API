using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WMS_CQRS_Business_Layer.DTOs;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.UserCommands
{
    public record RegisterUserCommand(dtoRegisterUser RegisterUser) : IRequest<IdentityResult>;
}
