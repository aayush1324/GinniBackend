﻿using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


        [HttpPost("addImage")]
        public async Task<IActionResult> AddImage([FromBody] List<string> urls)
        {
            if (urls == null || !urls.Any())
                return BadRequest();

            foreach (var url in urls)
            {
                var image = new Image { Url = url };
                await _authContext.Images.AddAsync(image);
            }

            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Add Image Success!"
            });
        }



        [HttpPost("addMultipleImage")]
        public async Task<IActionResult> AddImages([FromForm] List<IFormFile> images)
        {
            if (images == null || images.Count == 0)
                return BadRequest("No images provided");

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
    }
}
