using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.IssuingCommands;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.IssuingCommandHandlers
{
    public class DeleteIssuingCommandHandler(IIssuingRepo issuingRepo)
    : IRequestHandler<DeleteIssuingCommand, bool>
    {
        private readonly IIssuingRepo _issuingRepo = issuingRepo;

        public async Task<bool> Handle(DeleteIssuingCommand request, CancellationToken cancellationToken)
        {
            var result = await _issuingRepo.DeleteIssuing(request.IssueId);
            return result > 0;
        }
    }
}
