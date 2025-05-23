using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Queries.CategoryQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.CategoryQueriesHandlers
{
    public class GetAllCategoriesQueryHandler(ICategoryRepo categoryRepo) : IRequestHandler<GetAllCategoriesQuery, List<dtoCategory>>
    {
        private readonly ICategoryRepo _categoryRepo = categoryRepo;

        public async Task<List<dtoCategory>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var CategoriesList = await _categoryRepo.GetAllCategories();
            List<dtoCategory> categories = [];

            if (CategoriesList.Count > 0)
            {
                categories = CategoriesList.Select(c => new dtoCategory
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                }).ToList();
            }

            return categories;
        }
    }
}
