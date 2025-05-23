using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_CQRS_Business_Layer.CQRS.Queries.CategoryQueries
{
    public record GetAllCategoriesQuery : IRequest<List<dtoCategory>>;
}
