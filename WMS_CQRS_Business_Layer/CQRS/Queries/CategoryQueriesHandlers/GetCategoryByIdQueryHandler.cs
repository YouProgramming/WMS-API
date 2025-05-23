using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Queries.CategoryQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Data;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.CategoryQueriesHandlers
{
    public class GetCategoryByIdQueryHandler(ICategoryRepo categoryRepo) : IRequestHandler<GetCategoryByIdQuery, dtoCategory>
    {
        private readonly ICategoryRepo _categoryRepo = categoryRepo;
        public async Task<dtoCategory> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var C = await _categoryRepo.GetCategoryById(request.Id);

            dtoCategory category = new dtoCategory
            {
                CategoryId = C.CategoryId,
                CategoryName = C.CategoryName
            };
            return category;
        }
    }
}
