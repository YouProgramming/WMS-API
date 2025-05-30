using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.CQRS.Commands.IssuingCommands;
using WMS_CQRS_Business_Layer.CQRS.Commands.LogCommands;
using WMS_CQRS_Business_Layer.CQRS.Queries.ProductQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_CQRS_Business_Layer.Global;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsForIntegrationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("AllProductsForIntegration")]
        public async Task<IActionResult> GetAllProducts()
        {
            List<dtoProduct> result;
            try
            {
                result = await _mediator.Send(new GettAllProductsQuery());
            }
            catch
            {
                return StatusCode(500, "Internal Server Error, Please Try Again Later");
            }

            return Ok(new { result });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]

        [HttpGet("ProductImage/{productId:int}")]
        public async Task<IActionResult> GetImage(int productId)
        {
            try
            {
                var Product = await _mediator.Send(new GetProductByIdQuery(productId));

                var imageResult = clsGlobal.GetImageAsStreamResult(Product.ProductImagePath);

                if (imageResult == null)
                    return BadRequest(new { message = "Invalid or missing image path." });

                return imageResult; // FileStreamResult implements IActionResult
            }
            catch
            {
                return BadRequest(new { message = "Invalid or missing image path." });
            }
        }

        [HttpPost("IssueProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> AddNewIssuing(dtoIssuing issuing)
        {
            if (!issuing.Validate())
            {
                return BadRequest(new { Message = "Some fields are not valid." });
            }

            try
            {
                var id = await _mediator.Send(new AddNewIssuingCommand(issuing));
                //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //await _mediator.Send(new AddNewLogCommand(new dtoLog
                //{
                //    UserId = userId,
                //    Action = "AddIssuing",
                //    TimeStamp = DateTime.Now
                //}));
                return Ok(new { Message = "Issuing added successfully", Id = id });
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }
    }
}
