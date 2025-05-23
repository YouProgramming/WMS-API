using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.CQRS.Commands.CategoryCommands;
using WMS_CQRS_Business_Layer.CQRS.Commands.LogCommands;
using WMS_CQRS_Business_Layer.CQRS.Queries.CategoryQueries;
using WMS_CQRS_Business_Layer.DTOs;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllCategories()
        {
            List<dtoCategory> result;
            try
            {
                result = await _mediator.Send(new GetAllCategoriesQuery());
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            dtoCategory result;
            try
            {
                result = await _mediator.Send(new GetCategoryByIdQuery(id));
            }
            catch (Exception e)
            {
                if (e.Message == "Not Found")
                    return NotFound($"Category with ID: {id} Not Found");
                else
                    return StatusCode(500, "Internal Server Error, Please Try Again Later " + e.Message);
            }

            return Ok(new { result });
        }

        [HttpGet("ByName/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            dtoCategory result;
            try
            {
                result = await _mediator.Send(new GetCategoryByNameQuery(name));
            }
            catch (Exception e)
            {
                if (e.Message == "Not Found")
                    return NotFound($"Category with Name: {name} Not Found");
                else
                    return StatusCode(500, "Internal Server Error, Please Try Again Later " + e.Message);
            }

            return Ok(new { result });
        }

        [HttpPost("AddCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> AddNewCategory(dtoCategory category)
        {
            if (!category.isValid())
            {
                return BadRequest("Some Fields Are Not Valid.");
            }

            int id;
            try
            {
                id = await _mediator.Send(new AddNewCtegoryCommand(category));
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _mediator.Send(new AddNewLogCommand(new dtoLog
                {
                    UserId = userId,
                    Action = "AddCategory",
                    TimeStamp = DateTime.Now
                }));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error, Please Try Again Later " + e.Message);
            }

            return Ok(new { id });
        }

        [HttpPut("UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateCategory(dtoCategory category)
        {
            if (!category.isValid())
            {
                return BadRequest("Some Fields Are Not Valid.");
            }

            try
            {
                if (await _mediator.Send(new UpdateCategoryCommand(category)))
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    await _mediator.Send(new AddNewLogCommand(new dtoLog
                    {
                        UserId = userId,
                        Action = "UpdateCategory",
                        TimeStamp = DateTime.Now
                    }));
                    return Ok("Category Updated Successfully");
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Not Found")
                    return NotFound("Category with ID: " + category.CategoryId + " Not Found");
                else
                    return StatusCode(500, "Internal Server Error, Please Try Again Later " + e.Message);
            }

            return BadRequest(new { message = "Category Object Must Be Valid" });
        }

        [HttpDelete("DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                if (await _mediator.Send(new DeleteCategoryCommand(id)))
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    await _mediator.Send(new AddNewLogCommand(new dtoLog
                    {
                        UserId = userId,
                        Action = "DeleteCategory",
                        TimeStamp = DateTime.Now
                    }));
                    return Ok("Category Deleted Successfully");
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Not Found")
                    return NotFound("Category with ID: " + id + " Not Found");
                else
                    return StatusCode(500, "Internal Server Error, Please Try Again Later " + e.Message);
            }

            return BadRequest(new { message = "Category Id Must Be Valid" });
        }

    }
}
