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
    public class AddNewReceivingCommandHandler(IReceivingRepo receivingRepo)
    : IRequestHandler<AddNewReceivingCommand, int>
    {
        private readonly IReceivingRepo _receivingRepo = receivingRepo;

        public async Task<int> Handle(AddNewReceivingCommand request, CancellationToken cancellationToken)
        {
            Receiving receiving = new()
            {
                QuantityReceived = request.Receiving.QuantityReceived,
                ReceiveDate = request.Receiving.ReceiveDate,
                ProductId = request.Receiving.ProductId
            };

            return await _receivingRepo.InsertReceiving(receiving);
        }
    }
}
