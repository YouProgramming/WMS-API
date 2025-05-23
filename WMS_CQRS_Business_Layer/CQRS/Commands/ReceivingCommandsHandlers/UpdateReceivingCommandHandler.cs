using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.ReceivingCommands;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.ReceivingCommandHandlers
{
    public class UpdateReceivingCommandHandler(IReceivingRepo receivingRepo)
    : IRequestHandler<UpdateReceivingCommand, bool>
    {
        private readonly IReceivingRepo _receivingRepo = receivingRepo;

        public async Task<bool> Handle(UpdateReceivingCommand request, CancellationToken cancellationToken)
        {
            Receiving receiving = new()
            {
                ReceiveId = request.Receiving.ReceiveId,
                QuantityReceived = request.Receiving.QuantityReceived,
                ReceiveDate = request.Receiving.ReceiveDate,
                ProductId = request.Receiving.ProductId
            };

            var result = await _receivingRepo.UpdateReceiving(receiving);
            return result > 0;
        }
    }
}
