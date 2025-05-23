using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Queries.ReceivingsQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.ReceivingsQueryHandlers
{
    public class GetReceivingByIdQueryHandler(IReceivingRepo receivingRepo)
    : IRequestHandler<GetReceivingByIdQuery, dtoReceiving>
    {
        private readonly IReceivingRepo _receivingRepo = receivingRepo;

        public async Task<dtoReceiving> Handle(GetReceivingByIdQuery request, CancellationToken cancellationToken)
        {
            var r = await _receivingRepo.GetReceivingById(request.ReceiveId);

            return new dtoReceiving
            {
                ReceiveId = r.ReceiveId,
                QuantityReceived = r.QuantityReceived,
                ReceiveDate = r.ReceiveDate,
                ProductId = r.ProductId
            };
        }
    }
}
