using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Queries.ProductQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.ProductQueriesHandlers
{
    public class GetProductByIdQueryHandler(IProductRepo productRepo) : IRequestHandler<GetProductByIdQuery, dtoProduct>
    {
        private readonly IProductRepo _productRepo = productRepo;

        public async Task<dtoProduct> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var p = await _productRepo.GetProductById(request.Id);

            dtoProduct product = new dtoProduct {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                QuantityInStock = p.QuantityInStock,
                CategoryId = p.CategoryId,
                ProductImagePath = p.ProductImagePath,
            };

            return product;
        }
    }
}
