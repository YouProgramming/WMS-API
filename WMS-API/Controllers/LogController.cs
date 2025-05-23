using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.CQRS.Commands.LogCommands;
using WMS_CQRS_Business_Layer.CQRS.Queries.LogsQueries;
using WMS_CQRS_Business_Layer.DTOs;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllLogs()
        {
            List<dtoLog> result;
            try
            {
                result = await _mediator.Send(new GetAllLogsQuery());
            }
            catch
            {
                return StatusCode(500, "Internal Server Error, Please Try Again Later");
            }
            return Ok(new { result });
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]

        public async Task<IActionResult> GetLogById(int id)
        {
            dtoLog result;
            try
            {
                result = await _mediator.Send(new GetLogByIdQuery(id));
            }
            catch (Exception e)
            {
                if (e.Message == "Not Found")
                    return NotFound($"Log with ID: {id} Not Found");
                else
                    return StatusCode(500, "Internal Server Error " + e.Message);
            }

            return Ok(new { result });
        }

        [HttpPost("AddLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]

        public async Task<IActionResult> AddNewLog(dtoLog log)
        {
            if (!log.Validate())
                return BadRequest("Some Fields Are Not Valid.");

            try
            {
                int id = await _mediator.Send(new AddNewLogCommand(log));
                return Ok(new { id });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }

        [HttpPut("UpdateLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]

        public async Task<IActionResult> UpdateLog(dtoLog log)
        {
            if (!log.Validate())
                return BadRequest("Some Fields Are Not Valid.");

            try
            {
                if (await _mediator.Send(new UpdateLogCommand(log)))
                    return Ok("Log Updated Successfully");
            }
            catch (Exception e)
            {
                if (e.Message == "Not Found")
                    return NotFound($"Log with ID: {log.LogId} Not Found");
                else
                    return StatusCode(500, "Internal Server Error " + e.Message);
            }

            return BadRequest("Invalid Log Object");
        }

        [HttpDelete("DeleteLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            try
            {
                if (await _mediator.Send(new DeleteLogCommand(id)))
                    return Ok("Log Deleted Successfully");
            }
            catch (Exception e)
            {
                if (e.Message == "Not Found")
                    return NotFound($"Log with ID: {id} Not Found");
                else
                    return StatusCode(500, "Internal Server Error " + e.Message);
            }

            return BadRequest("Log ID Must Be Valid");
        }
    }
}
