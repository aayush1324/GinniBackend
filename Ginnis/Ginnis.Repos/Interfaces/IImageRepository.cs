using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface IImageRepository
    {
        Task<IActionResult> AddMultipleImages(List<IFormFile> images, Guid productId);
        
        Task<IActionResult> GetImagesByProductId(Guid productId);
    }
}
