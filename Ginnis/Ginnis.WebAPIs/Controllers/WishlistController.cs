using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public WishlistController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }


        // POST: api/Wishlist/addWishlist
        [HttpPost("addWishlist")]
        public async Task<IActionResult> AddWishlist([FromBody] WishlistItem wishlist)
        {
            if (wishlist == null)
                return BadRequest();

            try
            {
                // Check if the product already exists in the cart
                var existingWishlistItem = await _authContext.WishlistItems
                    .FirstOrDefaultAsync(c => c.ProductName == wishlist.ProductName);

                var existingWishList = await _authContext.ProductLists
                    .FirstOrDefaultAsync(c=>c.ProductName == wishlist.ProductName);

                if (existingWishlistItem != null)
                {
                    // Item already exists, return conflict
                    return Conflict("Item already exists in the wishlist.");
                }
                else
                {
                    // If the product doesn't exist, add it to the cart with a default quantity of 1
                    wishlist.Quantity = 1;
                    wishlist.TotalPrice = wishlist.Price;
                    wishlist.WishlistStatus = true; // Update the wishlist status
                    await _authContext.WishlistItems.AddAsync(wishlist);
                }

                wishlist.Created_at = DateTime.Now;


                existingWishList.WishlistStatus = true;
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Item added to Wishlist successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("getWishlist/{userId}")]
        public async Task<IActionResult> GetWishlist(Guid userId)
        {
            try
            {
                var wishlist = await _authContext.WishlistItems
                    .Where(w => w.UserId == userId)
                    .ToListAsync();

                if (wishlist == null || wishlist.Count == 0)
                {
                    return NotFound();
                }

                return Ok(wishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // Update your API controller to handle updating the wishlist status
        [HttpPost("updateWishlistStatus")]
        public async Task<IActionResult> UpdateWishlistStatus([FromBody] WishlistItem wishlist)
        {
            var existingWishlist = await _authContext.WishlistItems.FindAsync(wishlist.Id);
            if (existingWishlist == null)
            {
                return NotFound();
            }

            existingWishlist.WishlistStatus = wishlist.WishlistStatus;

            try
            {
                await _authContext.SaveChangesAsync();
                return Ok(new { Message = "Wishlist status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("updateWishlistQuantity/{id}")]
        public async Task<IActionResult> UpdateWishlistQuantity(Guid id, [FromBody] WishlistItem wishlist)
        {
            if (id != wishlist.Id)
            {
                return BadRequest();
            }

            var existingWishlisItem = await _authContext.WishlistItems.FindAsync(id);

            if (existingWishlisItem == null)
            {
                return NotFound();
            }

            // Calculate the new total price based on the updated quantity
            existingWishlisItem.Quantity = wishlist.Quantity;
            existingWishlisItem.TotalPrice = existingWishlisItem.Quantity * existingWishlisItem.Price;


            _authContext.Entry(existingWishlisItem).State = EntityState.Modified;

            try
            {
                await _authContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return NoContent();
        }


        [HttpDelete("deleteItem/{userId}/{productId}")]
        public async Task<IActionResult> RemoveWishlistItem(string userId, string productId)
        {
            if (!Guid.TryParse(userId, out Guid userIdGuid) || !Guid.TryParse(productId, out Guid productIdGuid))
            {
                return BadRequest("Invalid user or product ID format");
            }

            // Find the wishlist item for the specified user and product
            var wishlistItem = await _authContext.WishlistItems.FirstOrDefaultAsync(wi => wi.UserId == userIdGuid && wi.ProductId == productIdGuid);

            if (wishlistItem == null)
            {
                return NotFound();
            }

            // Update the wishlist status of the associated product
            var productList = await _authContext.ProductLists.FindAsync(productIdGuid);
            if (productList != null)
            {
                productList.WishlistStatus = false;
            }

            // Remove the wishlist item
            _authContext.WishlistItems.Remove(wishlistItem);
            await _authContext.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("deleteAllItem")]
        public async Task<IActionResult> EmptyWishlist()
        {
            var productList = await _authContext.ProductLists.ToListAsync();

            foreach (var product in productList)
            {
                product.WishlistStatus = false;
            }

            _authContext.WishlistItems.RemoveRange(_authContext.WishlistItems); // Remove all Wishlist items

            await _authContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
