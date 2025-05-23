using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.ReceivingCommands;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.ReceivingCommandHandlers
{
    public class DeleteReceivingCommandHandler(IReceivingRepo receivingRepo)
    : IRequestHandler<DeleteReceivingCommand, bool>
    {
        private readonly IReceivingRepo _receivingRepo = receivingRepo;

        public async Task<bool> Handle(DeleteReceivingCommand request, CancellationToken cancellationToken)
        {
            var result = await _receivingRepo.DeleteReceiving(request.ReceiveId);
            return result > 0;
        }
    }
}
