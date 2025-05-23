using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.IssuingCommands;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.IssuingCommandHandlers
{
    public class UpdateIssuingCommandHandler(IIssuingRepo issuingRepo)
    : IRequestHandler<UpdateIssuingCommand, bool>
    {
        private readonly IIssuingRepo _issuingRepo = issuingRepo;

        public async Task<bool> Handle(UpdateIssuingCommand request, CancellationToken cancellationToken)
        {
            Issuing issuing = new()
            {
                IssueId = request.Issuing.IssueId,
                QuantityIssued = request.Issuing.QuantityIssued,
                IssueDate = request.Issuing.IssueDate,
                ProductId = request.Issuing.ProductId
            };

            var result = await _issuingRepo.UpdateIssuing(issuing);
            return result > 0;
        }
    }
}
