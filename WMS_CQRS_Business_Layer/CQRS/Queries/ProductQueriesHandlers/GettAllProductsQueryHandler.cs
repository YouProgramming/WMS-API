using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Queries.ProductQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.ProductQueriesHandlers
{
    public class GettAllProductsQueryHandler(IProductRepo productRepo) : IRequestHandler<GettAllProductsQuery, List<dtoProduct>>
    {
        private readonly IProductRepo _productRepo = productRepo;

        public async Task<List<dtoProduct>> Handle(GettAllProductsQuery request, CancellationToken cancellationToken)
        {
            var ProductsList = await _productRepo.GetAllProducts();
            List<dtoProduct> products = [];

            if (ProductsList.Count > 0)
            {
                products = ProductsList.Select(p => new dtoProduct
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    UnitPrice = p.UnitPrice,
                    QuantityInStock = p.QuantityInStock,
                    CategoryId = p.CategoryId,
                    ProductImagePath = p.ProductImagePath,
                }).ToList();
            }


            return products ?? [];
        }


        
    }
}
