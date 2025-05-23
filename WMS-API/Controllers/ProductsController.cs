using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WMS_CQRS_Business_Layer.CQRS.Commands.LogCommands;
using WMS_CQRS_Business_Layer.CQRS.Commands.ProductsCommands;
using WMS_CQRS_Business_Layer.CQRS.Queries.ProductQueries;
using WMS_CQRS_Business_Layer.DTOs;
using WMS_CQRS_Business_Layer.Global;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        
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

        [HttpGet("Id/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]

        public async Task<IActionResult> GetProductBy(int Id)
        {
            dtoProduct result;
            try
            {
                result = await _mediator.Send(new GetProductByIdQuery(Id));

            }
            catch(Exception e)
            {
                if (e.Message == "Not Found")
                    return NotFound("Product with ID: " + Id + " Not Found");
                else
                    return StatusCode(500, "Internal Server Error, Please Try Again Later " + e.Message);
            }

            return Ok(new { result });
        }

        [HttpGet("{Name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]

        public async Task<IActionResult> GetProductByName(string Name)
        {
            dtoProduct result;
            try
            {
                result = await _mediator.Send(new GetProductByNameQuery(Name));
            }
            catch(Exception e)
            {
                if (e.Message == "Not Found")
                    return NotFound("Product with Name: " + Name + " Not Found");
                else
                    return StatusCode(500, "Internal Server Error, Please Try Again Later " + e.Message);
            }

            return Ok(new { result });
        }

        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]

        public async Task<IActionResult> AddNewProduct([FromForm]dtoProduct product)
        {
            if (!product.Validate())
            {
                return BadRequest("Some fields are not valid.");
            }

            try
            {
                int id = await _mediator.Send(new AddNewProductCommand(product));

                if (product.ProductImage != null)
                {
                    string? relativePath = await clsGlobal.SavePictureAsync(product.ProductImage, "Product-Image-folder");

                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        await _mediator.Send(new UpdateProductPictureCommand(relativePath, id));
                    }
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _mediator.Send(new AddNewLogCommand(new dtoLog
                {
                    UserId = userId,
                    Action = "AddProduct",
                    TimeStamp = DateTime.Now
                }));
                return Ok(new { Id = id });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error, please try again later. " + e.Message);
            }

        }

        [HttpPut("UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]

        public async Task<IActionResult> UpdateProduct([FromForm] dtoProduct product)
        {
            if (!product.Validate())
            {
                return BadRequest("Some fields are not valid.");
            }

            try
            {
                bool updated = await _mediator.Send(new UpdateProductCommand(product));

                if (!updated)
                    return BadRequest(new { message = "Product update failed. Please check the data." });

                if (product.ProductImage != null)
                {
                    string? relativePath = await clsGlobal.SavePictureAsync(product.ProductImage, "Product-Image-folder");

                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        await _mediator.Send(new UpdateProductPictureCommand(relativePath, product.ProductId));
                    }
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _mediator.Send(new AddNewLogCommand(new dtoLog
                {
                    UserId = userId,
                    Action = "UpdateProduct",
                    TimeStamp = DateTime.Now
                }));

                return Ok("Product updated successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Product with ID: {product.ProductId} not found.");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error. Please try again later. " + e.Message);
            }

        }

        [HttpDelete("DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _mediator.Send(new GetProductByIdQuery(id));

                if(product.ProductImagePath != null && product.ProductImagePath != "")
                {
                    bool fileDeleted = clsGlobal.DeleteFile(product.ProductImagePath);
                    if (!fileDeleted)
                    {
                        return StatusCode(500, $"Failed to delete image file for product ID: {id}");
                    }
                }
                bool deleted = await _mediator.Send(new DeleteProductCommand(id));

                if (deleted)
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    await _mediator.Send(new AddNewLogCommand(new dtoLog
                    {
                        UserId = userId,
                        Action = "AddProduct",
                        TimeStamp = DateTime.Now
                    }));
                    return Ok("Product deleted successfully.");
                }
                else
                    return BadRequest("Failed to delete product from the database.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Product with ID: {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later. " + ex.Message);
            }
        }

        

    }
}
