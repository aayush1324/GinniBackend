using Ginnis.Domains.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface IProductRepository
    {

        Task<IActionResult> GetProductsWithImage(Guid userID);

        Task<IActionResult> GetProductsWithImages();

        Task<IActionResult> AddImageToProduct(ProductList product, IFormFile image);

        Task<IActionResult> EditProduct(Guid productId, ProductList updatedProduct, IFormFile image);

        Task<IActionResult> DeleteProduct(Guid productId);

        IActionResult Search(string term);

        Task<IActionResult> GetProduct();

        Task<IActionResult> GetProductByName(string productName);

        Task<IActionResult> GetProductImage(Guid productId);
    }
}
