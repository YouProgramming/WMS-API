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
    public class GetAllReceivingsQueryHandler(IReceivingRepo receivingRepo)
    : IRequestHandler<GetAllReceivingsQuery, List<dtoReceiving>>
    {
        private readonly IReceivingRepo _receivingRepo = receivingRepo;

        public async Task<List<dtoReceiving>> Handle(GetAllReceivingsQuery request, CancellationToken cancellationToken)
        {
            var list = await _receivingRepo.GetAllReceivings();

            return list.Select(r => new dtoReceiving
            {
                ReceiveId = r.ReceiveId,
                QuantityReceived = r.QuantityReceived,
                ReceiveDate = r.ReceiveDate,
                ProductId = r.ProductId
            }).ToList();
        }
    }
}
