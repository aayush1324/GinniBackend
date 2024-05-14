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

        [HttpPost("addCart")]
        public async Task<IActionResult> AddCart([FromBody] CartList cart)
        {
            return await _cartRepository.AddCart(cart);
        }

        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart([FromBody] CartList cart)
        {
            return await _cartRepository.AddToCart(cart);
        }

        [HttpGet("getCart")]
        public async Task<IActionResult> GetCart()
        {
            return await _cartRepository.GetCart();
        }

        [HttpGet("getCarts/{userId}")]
        public async Task<IActionResult> GetCart(Guid userId)
        {
            return await _cartRepository.GetCart(userId);
        }

        [HttpPut("updateCartQuantity/{id}")]
        public async Task<IActionResult> UpdateCartQuantity(Guid id, [FromBody] CartList cart)
        {
            return await _cartRepository.UpdateCartQuantity(id, cart);
        }

        [HttpDelete("deleteItem/{id}")]
        public async Task<IActionResult> RemoveCartItem(Guid id)
        {
            return await _cartRepository.RemoveCartItem(id);
        }

        [HttpDelete("deleteAllItem/{userId}")]
        public async Task<IActionResult> EmptyCart(Guid userId)
        {
            return await _cartRepository.EmptyCart(userId);
        }

        [HttpPost("addToWishlist")]
        public async Task<IActionResult> AddToWishlist([FromBody] CartList item)
        {
            return await _cartRepository.AddToWishlist(item);
        }
    }
}
