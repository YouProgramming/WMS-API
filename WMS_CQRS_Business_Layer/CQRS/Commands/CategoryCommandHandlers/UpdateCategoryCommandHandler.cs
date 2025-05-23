using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.CategoryCommands;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.CategoryCommandHandlers
{
    internal class UpdateCategoryCommandHandler(ICategoryRepo categoryRepo) : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ICategoryRepo _categoryRepo = categoryRepo;
        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = new()
            {
                CategoryId = request.Category.CategoryId,
                CategoryName = request.Category.CategoryName
            };

            return await _categoryRepo.UpdateCategory(category) > 0;    
        }
    }
}
