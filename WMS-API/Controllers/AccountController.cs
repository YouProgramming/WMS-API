using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.CQRS.Commands.UserCommands;
using WMS_CQRS_Business_Layer.CQRS.Queries.UserQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_CQRS_Business_Layer.Global;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser([FromForm]dtoRegisterUser newUser)
        {
            if (!newUser.Validate())
                return BadRequest(new { Message = "Some fields are not valid." });

            try
            {
                var identityResult = await _mediator.Send(new RegisterUserCommand(newUser));

                if (identityResult.Succeeded)
                {
                    string? relativePath = null;

                    if (newUser.ProfilePicture != null)
                        relativePath = await clsGlobal.SavePictureAsync(newUser.ProfilePicture, "pfp-folder");

                    if (relativePath != null)
                        await _mediator.Send(new UpdateUserPfpCommand(relativePath, newUser.Username));

                    return Ok("User registered successfully.");
                }

                return BadRequest(new
                {
                    Message = "Registration failed.",
                    Errors = identityResult.Errors.Select(e => e.Description)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while registering the user. " + ex.Message);
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(dtoLogin login)
        {
            if (!login.IsValid())
                return BadRequest(new { Message = "Some fields are not valid." });

            try
            {
                string token = await _mediator.Send(new LoginCommand(login));

                if (string.IsNullOrWhiteSpace(token))
                    return BadRequest(new { Message = "Invalid username or password." });

                var user = await _mediator.Send(new GetUserByUsernameQuery(login.Username));

                return Ok(new { token, user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while logging in. " + ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet("images")]
        public IActionResult GetImage([FromQuery] string relativePath)
        {
            try
            {
                var imageResult = clsGlobal.GetImageAsStreamResult(relativePath);

                if (imageResult == null)
                    return BadRequest(new { message = "Invalid or missing image path." });

                return imageResult; // FileStreamResult implements IActionResult
            }
            catch
            {
                return BadRequest(new { message = "Invalid or missing image path." });
            }
        }
    }
}
