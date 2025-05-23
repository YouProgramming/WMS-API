using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.CQRS.Commands.ProductsCommands;
using WMS_Repository_Data_Layer.Repository.IRepos;

namespace WMS_CQRS_Business_Layer.CQRS.Commands.ProductsCommandsHandlers
{
    internal class DeleteProductCommandHandler(IProductRepo productRepo) : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepo _productRepo = productRepo;
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return (await _productRepo.DeleteProduct(request.Id) > 0);
        }
    }
}
