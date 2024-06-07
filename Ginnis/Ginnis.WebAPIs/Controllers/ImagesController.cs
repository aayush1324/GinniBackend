using Ginnis.Repos.Interfaces;
using Ginnis.Repos.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ginnis.WebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("addMultipleImage")]
        public async Task<IActionResult> AddMultipleImages([FromForm] List<IFormFile> images, [FromForm] Guid productId)
        {
            // Validation and saving images handled by repository
            return await _imageRepository.AddMultipleImages(images, productId);
        }



        [HttpGet("getImagesByProductId/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(Guid productId)
        {
            // Retrieving images by product ID handled by repository
            return await _imageRepository.GetImagesByProductId(productId);
        }
    }
}
