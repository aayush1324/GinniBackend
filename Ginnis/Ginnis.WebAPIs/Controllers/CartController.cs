using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public CartController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }


        // POST: api/cart/addCart
        [HttpPost("addCart")]
        public async Task<IActionResult> AddCart([FromBody] CartList cart)
        {
            if (cart == null)
                return BadRequest();

            try
            {
                // Check if the product already exists in the cart
                var existingCartItem = await _authContext.CartLists
                    .FirstOrDefaultAsync(c => c.ProductName == cart.ProductName);

                if (existingCartItem != null)
                {
                    // If the product already exists, increment its quantity
                    existingCartItem.Quantity += 1;
                    existingCartItem.TotalPrice = existingCartItem.Quantity * existingCartItem.Price;
                }
                else
                {
                    // If the product doesn't exist, add it to the cart with a default quantity of 1
                    cart.Quantity = 1;
                    cart.TotalPrice = cart.Price;
                    await _authContext.CartLists.AddAsync(cart);
                }

                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Item added to cart successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // GET: api/cart/getCart
        [HttpGet("getCart")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var cartList = await _authContext.CartLists.ToListAsync();

                if (cartList == null || cartList.Count == 0)
                {
                    return NotFound();
                }

                return Ok(cartList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("updateCartQuantity/{id}")]
        public async Task<IActionResult> UpdateCartQuantity(Guid id, [FromBody] CartList cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            var existingCartItem = await _authContext.CartLists.FindAsync(id);

            if (existingCartItem == null)
            {
                return NotFound();
            }

            // Calculate the new total price based on the updated quantity
            existingCartItem.Quantity = cart.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * existingCartItem.Price;


            _authContext.Entry(existingCartItem).State = EntityState.Modified;

            try
            {
                await _authContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
              
            }

            return NoContent();
        }


        [HttpDelete("deleteItem/{id}")]
        public async Task<IActionResult> RemoveCartItem(Guid id)
        {
            var cartItem = await _authContext.CartLists.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _authContext.CartLists.Remove(cartItem);
            await _authContext.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("deleteAllItem")]
        public async Task<IActionResult> EmptyCart()
        {
            _authContext.CartLists.RemoveRange(_authContext.CartLists); // Remove all cart items
            await _authContext.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("addToWishlist")]
        public async Task<IActionResult> AddToWishlist([FromBody] CartList item)
        {
            if (item == null)
                return BadRequest();

            try
            {
                // Remove item from cart
                var existingCartItem = await _authContext.CartLists
                    .FirstOrDefaultAsync(c => c.ProductName == item.ProductName);

                if (existingCartItem != null)
                {
                    _authContext.CartLists.Remove(existingCartItem);
                    await _authContext.SaveChangesAsync();
                }

                // Example:
                var wishlistItem = new WishlistItem
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,   
                    Price = item.Price,
                    TotalPrice = item.TotalPrice,
                    // Set other properties as needed
                };

                await _authContext.WishlistItems.AddAsync(wishlistItem);
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Item added to wishlist successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}

