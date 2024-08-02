using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        //[Authorize]
        [HttpGet("getProductsWithImage/{userID}")]
        public async Task<IActionResult> GetProductsWithImage(Guid userID)
        {
            return await _productRepository.GetProductsWithImage(userID);
        }


        [HttpGet("getProductsWithImages")]
        public async Task<IActionResult> GetProductsWithImages()
        {
            return await _productRepository.GetProductsWithImages();
        }


        //[Authorize(Roles = "Admin")]
        [HttpPost("addProductsWithImages")]
        public async Task<IActionResult> AddImageToProduct([FromForm] ProductList product, IFormFile image)
        {
            return await _productRepository.AddImageToProduct(product, image);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("editProduct/{productId}")]
        public async Task<IActionResult> EditProduct(Guid productId, [FromForm] ProductList updatedProduct, IFormFile image)
        {
            return await _productRepository.EditProduct(productId, updatedProduct, image);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            return await _productRepository.DeleteProduct(productId);
        }


        [HttpGet("search")]
        public IActionResult Search(string term)
        {
            return _productRepository.Search(term);
        }


        [HttpGet("getProduct")]
        public async Task<IActionResult> GetProduct()
        {
            return await _productRepository.GetProduct();
        }



        [HttpGet("getProductName/{productName}")]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            return await _productRepository.GetProductByName(productName);
        }



        [HttpGet("getImage/{productId}")]
        public async Task<IActionResult> GetProductImage(Guid productId)
        {
            return await _productRepository.GetProductImage(productId);
        }

    }
}
