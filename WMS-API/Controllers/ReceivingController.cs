using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.CQRS.Commands.ReceivingCommands;
using WMS_CQRS_Business_Layer.CQRS.Queries.ReceivingsQueries;
using WMS_CQRS_Business_Layer.DTOs;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReceivingController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllReceivings()
        {
            try
            {
                var result = await _mediator.Send(new GetAllReceivingsQuery());
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return StatusCode(500, new { Message = "Internal Server Error, Please Try Again Later", Details = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetReceivingById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetReceivingByIdQuery(id));
                if (result == null)
                    return NotFound(new { Message = $"Receiving with ID: {id} Not Found" });

                return Ok(new { result });
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

        [HttpPost("AddReceiving")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> AddNewReceiving(dtoReceiving receiving)
        {
            if (!receiving.Validate())
                return BadRequest(new { Message = "Some fields are not valid." });

            try
            {
                var id = await _mediator.Send(new AddNewReceivingCommand(receiving));
                return Ok(new { Message = "Receiving added successfully", Id = id });
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

        [HttpPut("UpdateReceiving")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateReceiving(dtoReceiving receiving)
        {
            if (!receiving.Validate())
                return BadRequest(new { Message = "Some fields are not valid." });

            try
            {
                var success = await _mediator.Send(new UpdateReceivingCommand(receiving));
                if (!success)
                    return NotFound(new { Message = $"Receiving with ID: {receiving.ReceiveId} Not Found" });

                return Ok(new { Message = "Receiving updated successfully" });
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

        [HttpDelete("DeleteReceiving/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteReceiving(int id)
        {
            try
            {
                var success = await _mediator.Send(new DeleteReceivingCommand(id));
                if (!success)
                    return NotFound(new { Message = $"Receiving with ID: {id} Not Found" });

                return Ok(new { Message = "Receiving deleted successfully" });
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

       
    }
}
