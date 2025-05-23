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
    public class AddNewIssuingCommandHandler(IIssuingRepo issuingRepo)
    : IRequestHandler<AddNewIssuingCommand, int>
    {
        private readonly IIssuingRepo _issuingRepo = issuingRepo;

        public async Task<int> Handle(AddNewIssuingCommand request, CancellationToken cancellationToken)
        {
            Issuing issuing = new()
            {
                QuantityIssued = request.Issuing.QuantityIssued,
                IssueDate = request.Issuing.IssueDate,
                ProductId = request.Issuing.ProductId
            };

            
            return await _issuingRepo.InsertIssuing(issuing);
        }
    }
}
