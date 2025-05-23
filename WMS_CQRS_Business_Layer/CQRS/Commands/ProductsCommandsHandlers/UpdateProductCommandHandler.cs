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
    public class UpdateProductCommandHandler(IProductRepo productRepo) : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepo _pr = productRepo;
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new()
            {
                ProductId = request.Product.ProductId,
                ProductName = request.Product.ProductName,
                Description = request.Product.Description,
                UnitPrice = request.Product.UnitPrice,
                QuantityInStock = request.Product.QuantityInStock,
                CategoryId = request.Product.CategoryId,
            };

            return await _pr.UpdateProduct(product) > 0;
        }
    }
}
