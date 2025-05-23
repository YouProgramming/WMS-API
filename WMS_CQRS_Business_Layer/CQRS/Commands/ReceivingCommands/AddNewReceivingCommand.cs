﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.DTOs;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.ReceivingCommands
{
    public record AddNewReceivingCommand(dtoReceiving Receiving) : IRequest<int>;
}
