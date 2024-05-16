using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _authContext;

        public ImageRepository(AppDbContext context)
        {
            _authContext = context;
        }



        public async Task<IActionResult> AddMultipleImages(List<IFormFile> images, Guid productId)
        {
            if (images == null || images.Count == 0)
                return new BadRequestObjectResult("No images provided");

            var product = await _authContext.ProductLists.FindAsync(productId);
            if (product == null)
                return new BadRequestObjectResult("Invalid product ID");

            foreach (var image in images)
            {
                if (image == null || image.Length == 0)
                    continue;

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    var newImage = new Image
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        ProfileImage = memoryStream.ToArray(),
                        ImageData = Convert.ToBase64String(memoryStream.ToArray()),
                        Created_at = DateTime.Now
                    };

                    _authContext.Images.Add(newImage);
                }
            }

            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new
            {
                Message = "Images added successfully!"
            });
        }



        public async Task<IActionResult> GetImagesByProductId(Guid productId)
        {
            try
            {
                var images = await _authContext.Images.Where(img => img.ProductId == productId).ToListAsync();
                return new OkObjectResult(images);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
