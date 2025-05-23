using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.CQRS.Queries.CategoryQueries;
using WMS_CQRS_Business_Layer.CQRS.Queries.ReportsQueries;
using WMS_CQRS_Business_Layer.DTOs;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("StockMovementReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> StockMovementReport()
        {
            List<dtoStockMovementReport> result;
            try
            {
                result = await _mediator.Send(new StockMovementReportQuery());
            }
            catch
            {
                return StatusCode(500, "Internal Server Error, Please Try Again Later");
            }

            return Ok(new { result });
        }

        [HttpGet("StockOverviewReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> StockOverviewReport()
        {
            List<dtoStockOverviewReport> result;
            try
            {
                result = await _mediator.Send(new StockOverviewReportQuery());
            }
            catch
            {
                return StatusCode(500, "Internal Server Error, Please Try Again Later");
            }

            return Ok(new { result });
        }

    }
}
