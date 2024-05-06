using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Google.Api;
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


        [HttpPost("addProductsWithImages")]
        public async Task<IActionResult> AddImageToProduct([FromForm] ProductList product, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Invalid file");

            if (product == null)
                return BadRequest();

            product.Created_at = DateTime.Now;


            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                product.ProfileImage = memoryStream.ToArray();
                product.ImageData = Convert.ToBase64String(memoryStream.ToArray());               
            }

            var productAdded = _authContext.ProductLists.Add(product);
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                ProductId = productAdded.Entity.Id.ToString(),
                Message = "Product Added Successfully!"
            });
        }


        [HttpGet("getProductsWithImages")]
        public async Task<IActionResult> GetProductsWithImages()
        {
            var products = await _authContext.ProductLists.ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);

        }


        [HttpGet("getImage/{productId}")]
        public async Task<IActionResult> GetProductImage(Guid productId)
        {
            var product = await _authContext.ProductLists.FindAsync(productId);
            if (product == null || product.ProfileImage == null)
            {
                return NotFound();
            }

            return File(product.ProfileImage, "image/jpeg"); // Adjust the content type as per your image format
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
        public async Task<IActionResult> EditProduct(Guid productId, [FromForm] ProductList updatedProduct, IFormFile image)
        {
            try
            {
                // Find the product by its unique identifier
                var product = await _authContext.ProductLists.FindAsync(productId);

                if (product == null)
                {
                    return NotFound(); // Product not found
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

                product.Modified_at = DateTime.Now;

                // Handle image update if provided
                if (image != null)
                {
                    // Process the image here
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        product.ProfileImage = memoryStream.ToArray();
                        product.ImageData = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Product updated successfully!"
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
                
                // Retrieve images associated with the specified product ID
                var images = await _authContext.Images.Where(img => img.ProductId == productId).ToListAsync();


                if (product == null)
                {
                    return NotFound(); // Address not found
                }

                //if (images == null || images.Count == 0)
                //{
                //    return NotFound("No images found for the product");
                //}

                // Remove the address from the context
                _authContext.ProductLists.Remove(product);

                _authContext.Images.RemoveRange(images);


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
