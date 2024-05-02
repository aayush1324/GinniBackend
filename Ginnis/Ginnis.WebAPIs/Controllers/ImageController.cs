using Ginnis.Domains.Entities;
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


    }
}
