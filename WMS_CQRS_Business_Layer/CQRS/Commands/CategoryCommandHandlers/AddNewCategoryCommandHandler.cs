using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.CategoryCommands;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.CategoryCommandHandlers
{
    public class AddNewCategoryCommandHandler(ICategoryRepo categoryRepo) : IRequestHandler<AddNewCtegoryCommand, int>
    {
        private readonly ICategoryRepo _categoryRepo = categoryRepo;   
        public async Task<int> Handle(AddNewCtegoryCommand request, CancellationToken cancellationToken)
        {
            Category category = new()
            {
                CategoryName = request.Category.CategoryName
            };

            return await _categoryRepo.InsertCategory(category);
        }
    }
}
