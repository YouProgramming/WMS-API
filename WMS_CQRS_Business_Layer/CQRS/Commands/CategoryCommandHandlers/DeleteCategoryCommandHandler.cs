using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.CategoryCommands;
using WMS_Repository_Data_Layer.Repository.IRepos;
using WMS_Repository_Data_Layer.Repository.Repos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.CategoryCommandHandlers
{
    public class DeleteCategoryCommandHandler(ICategoryRepo categoryRepo) : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepo _categoryRepo = categoryRepo;

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            return !(await _categoryRepo.DeleteCategory(request.Id) < 0);
        }
    }
}
