using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.CQRS.Commands.LogCommands;
using WMS_CQRS_Business_Layer.CQRS.Commands.ProductsCommands;
using WMS_CQRS_Business_Layer.CQRS.Commands.UserCommands;
using WMS_CQRS_Business_Layer.CQRS.Queries.CategoryQueries;
using WMS_CQRS_Business_Layer.CQRS.Queries.ProductQueries;
using WMS_CQRS_Business_Layer.CQRS.Queries.UserQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_CQRS_Business_Layer.Global;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManageUsersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]

        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _mediator.Send(new GetAllUserQuery());
                return Ok(new { result });
            }
            catch (UnauthorizedAccessException)
            {
                
                return StatusCode(403, "Access Denied");
            }
            catch (Exception)
            {
                
                return StatusCode(500, "Internal Server Error, Please Try Again Later");
            }
        }

        [HttpGet("user/ByUsername/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                var result = await _mediator.Send(new GetUserByUsernameQuery(username));
                return Ok(new { result });
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with Username: {username} Not Found");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error, Please Try Again Later");
            }
        }

        [HttpDelete("DeleteUser/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByUsernameQuery(userName));

                if(user.ProfilePicturePath != null && user.ProfilePicturePath != "")
                {
                    bool fileDeleted = clsGlobal.DeleteFile(user.ProfilePicturePath);
                    if (!fileDeleted)
                    {
                        return StatusCode(500, $"Failed to delete image file for Username: {userName}");
                    }
                }
               

                bool deleted = await _mediator.Send(new DeleteUserCommand(userName));

                if (deleted)
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    await _mediator.Send(new AddNewLogCommand(new dtoLog
                    {
                        UserId = userId,
                        Action = "DeleteUser",
                        TimeStamp = DateTime.Now
                    }));
                    return Ok("User deleted successfully.");
                }
                else
                    return BadRequest("Failed to delete User from the database.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with Username: {userName} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later. " + ex.Message);
            }
        }

        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateUserAsync([FromForm]dtoRegisterUser updateUser)
        {
            if (updateUser.Username == null)
            {
                return BadRequest("Invalid user data or username mismatch.");
            }

           
            try
            {
                bool updated = await _mediator.Send(new UpdateUserCommand(updateUser));

                if (!updated)
                    return BadRequest(new { message = "User update failed. Please check the data." });

                if (updateUser.ProfilePicture != null)
                {
                    string? relativePath = await clsGlobal.SavePictureAsync(updateUser.ProfilePicture, "pfp-folder");

                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        await _mediator.Send(new UpdateUserPfpCommand(relativePath, updateUser.Username));
                    }
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _mediator.Send(new AddNewLogCommand(new dtoLog
                {
                    UserId = userId,
                    Action = "UpdateUser",
                    TimeStamp = DateTime.Now
                }));
                return Ok("User updated successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with Username: {updateUser.Username} not found.");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error. Please try again later. " + e.Message);
            }

            
        }

    }
}
