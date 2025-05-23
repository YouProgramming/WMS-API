using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.CQRS.Commands.IssuingCommands;
using WMS_CQRS_Business_Layer.CQRS.Queries.IssuingQueries;
using WMS_CQRS_Business_Layer.DTOs;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IssuingController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllIssuings()
        {
            try
            {
                var result = await _mediator.Send(new GetAllIssuingsQuery());
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                // Log exception for further analysis (use your logger)
                return StatusCode(500, new { Message = "Internal Server Error, Please Try Again Later", Details = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetIssuingById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetIssuingByIdQuery(id));
                if (result == null)
                {
                    return NotFound(new { Message = $"Issuing with ID: {id} Not Found" });
                }
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

        [HttpPost("AddIssuing")]
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
                return Ok(new { Message = "Issuing added successfully", Id = id });
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

        [HttpPut("UpdateIssuing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateIssuing(dtoIssuing issuing)
        {
            if (!issuing.Validate())
            {
                return BadRequest(new { Message = "Some fields are not valid." });
            }

            try
            {
                var success = await _mediator.Send(new UpdateIssuingCommand(issuing));
                if (!success)
                {
                    return NotFound(new { Message = $"Issuing with ID: {issuing.IssueId} Not Found" });
                }
                return Ok(new { Message = "Issuing updated successfully" });
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }


        [HttpDelete("DeleteIssuing/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteIssuing(int id)
        {
            try
            {
                var success = await _mediator.Send(new DeleteIssuingCommand(id));
                if (!success)
                {
                    return NotFound(new { Message = $"Issuing with ID: {id} Not Found" });
                }
                return Ok(new { Message = "Issuing deleted successfully" });
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

    }
}
