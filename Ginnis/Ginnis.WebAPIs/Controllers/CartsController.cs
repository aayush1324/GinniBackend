using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartsController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }


        
        [HttpPost("addToCarts/{userId}/{productId}")]
        public async Task<IActionResult> AddToCart(Guid userId, Guid productId)
        {
            return await _cartRepository.AddToCart(userId, productId);
        }



        [HttpGet("getCarts/{userId}")]
        public async Task<IActionResult> GetCart(Guid userId)
        {
            try
            {
                var cartDTOs = await _cartRepository.GetCart(userId);
                return Ok(cartDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpDelete("deleteItem/{userId}/{itemId}")]
        public async Task<IActionResult> RemoveCartItem(Guid userId, Guid itemId)
        {
            return await _cartRepository.RemoveCartItem(userId, itemId);
        }



        [HttpDelete("deleteAllItem/{userId}")]
        public async Task<IActionResult> EmptyCart(Guid userId)
        {
            return await _cartRepository.EmptyCart(userId);
        }



        [HttpPut("updateCartQuantity")]
        public async Task<IActionResult> UpdateCartQuantity([FromBody] CartDTO cart)
        {
            return await _cartRepository.UpdateCartQuantity(cart);
        }











        [HttpPost("addCart")]
        public async Task<IActionResult> AddCart([FromBody] CartList cart)
        {
            return await _cartRepository.AddCart(cart);
        }


        [HttpGet("getCart")]
        public async Task<IActionResult> GetCart()
        {
            return await _cartRepository.GetCart();
        }


        [HttpPost("addToWishlist")]
        public async Task<IActionResult> AddToWishlist([FromBody] CartList item)
        {
            return await _cartRepository.AddToWishlist(item);
        }
    }
}
