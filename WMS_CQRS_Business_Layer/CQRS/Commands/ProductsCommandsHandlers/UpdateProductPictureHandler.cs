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
    public class UpdateProductPictureHandler(IProductRepo productRepo) : IRequestHandler<UpdateProductPictureCommand, bool>
    {
        private readonly IProductRepo _productRepo = productRepo;
        public async Task<bool> Handle(UpdateProductPictureCommand request, CancellationToken cancellationToken)
        {
            return await _productRepo.SaveProductPicturePath(request.RelativePath, request.ProductId);

        }
    }
}
