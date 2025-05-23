using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.Global;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
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
