using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.ProductsCommands;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.ProductsCommandsHandlers
{
    public class AddNewProductCommandHandler(IProductRepo productRepo) : IRequestHandler<AddNewProductCommand, int>
    {
        private readonly IProductRepo _productRepo = productRepo;
        public async Task<int> Handle(AddNewProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new()
            {
                ProductName = request.Product.ProductName,
                Description = request.Product.Description,
                UnitPrice = request.Product.UnitPrice,
                QuantityInStock = request.Product.QuantityInStock,
                CategoryId = request.Product.CategoryId,
            };
            return await _productRepo.InsertProduct(product);
        }
    }
}
