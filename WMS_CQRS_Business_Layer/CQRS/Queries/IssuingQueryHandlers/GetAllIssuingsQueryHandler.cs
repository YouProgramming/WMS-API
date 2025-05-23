using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Queries.IssuingQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.IssuingQueryHandlers
{
    public class GetAllIssuingsQueryHandler(IIssuingRepo issuingRepo)
    : IRequestHandler<GetAllIssuingsQuery, List<dtoIssuing>>
    {
        private readonly IIssuingRepo _issuingRepo = issuingRepo;

        public async Task<List<dtoIssuing>> Handle(GetAllIssuingsQuery request, CancellationToken cancellationToken)
        {
            var list = await _issuingRepo.GetAllIssuings();

            return list.Select(i => new dtoIssuing
            {
                IssueId = i.IssueId,
                QuantityIssued = i.QuantityIssued,
                IssueDate = i.IssueDate,
                ProductId = i.ProductId
            }).ToList();
        }
    }

}
