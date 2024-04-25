using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public ProductController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductList product)
        {
            if (product == null)
                return BadRequest();

            await _authContext.ProductLists.AddAsync(product);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Add Product Success!"
            });
        }


        [HttpGet("getProduct")]
        public async Task<IActionResult> GetProduct()
        {
            var productlist = await _authContext.ProductLists.ToListAsync();

            if (productlist == null || productlist.Count == 0)
            {
                return NotFound();
            }

            return Ok(productlist);
        }


        [HttpGet("getProductName/{productName}")]
        public async Task<IActionResult> GetProductByName( string productName)
        {
            var product = await _authContext.ProductLists.FirstOrDefaultAsync(p => p.ProductName == productName);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        [HttpPut("editProduct/{productId}")]
        public async Task<IActionResult> EditProduct(Guid productId, [FromBody] ProductList updatedProduct)
        {
            try
            {
                // Find the address by its unique identifier
                var product = await _authContext.ProductLists.FindAsync(productId);

                if (product == null)
                {
                    return NotFound(); // Address not found
                }

                // Update product properties with the new values
                product.ProductName = updatedProduct.ProductName;
                product.Url = updatedProduct.Url;
                product.Price = updatedProduct.Price;
                product.Discount = updatedProduct.Discount;
                product.DeliveryPrice = updatedProduct.DeliveryPrice;
                product.Quantity = updatedProduct.Quantity;
                product.Description = updatedProduct.Description;
                product.Category = updatedProduct.Category;
                product.Subcategory = updatedProduct.Subcategory;
                product.Weight = updatedProduct.Weight;
                product.Status = updatedProduct.Status;

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Update Product Success!"
                }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpDelete("deleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            try
            {
                var product = await _authContext.ProductLists.FindAsync(productId);

                if (product == null)
                {
                    return NotFound(); // Address not found
                }

                // Remove the address from the context
                _authContext.ProductLists.Remove(product);

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "DeleteProduct Success!"
                }); // Address successfully deleted
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }






    }
}
