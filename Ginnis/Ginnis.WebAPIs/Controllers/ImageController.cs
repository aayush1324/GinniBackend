using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public ImageController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }


        [HttpPost("addMultipleImage")]
        public async Task<IActionResult> AddMultipleImages([FromForm] List<IFormFile> images, [FromForm] Guid productId)
        {
            if (images == null || images.Count == 0)
                return BadRequest("No images provided");

            // Find the product based on the provided product ID
            var product = await _authContext.ProductLists.FindAsync(productId);
            if (product == null)
                return BadRequest("Invalid product ID");

            foreach (var image in images)
            {
                if (image == null || image.Length == 0)
                    continue; // Skip empty images

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    var newImage = new Image
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId, // Associate the image with the specified product
                        ProfileImage = memoryStream.ToArray(),
                        ImageData = Convert.ToBase64String(memoryStream.ToArray())
                    };

                    _authContext.Images.Add(newImage);
                }
            }

            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Images added successfully!"
            });
        }


        [HttpGet("getImagesByProductId/{productId}")]
        public IActionResult GetImagesByProductId(Guid productId)
        {
            try
            {
                // Retrieve images associated with the specified product ID
                var images = _authContext.Images.Where(img => img.ProductId == productId).ToList();

                return Ok(images);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
