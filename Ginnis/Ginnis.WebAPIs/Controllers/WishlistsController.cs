using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistsController(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }


        [HttpPost("addWishlist")]
        public async Task<IActionResult> AddWishlist([FromBody] WishlistItem wishlist)
        {
            return await _wishlistRepository.AddWishlistItem(wishlist);
        }


        [HttpGet("getWishlists/{userId}")]
        public async Task<IActionResult> GetWishlist(Guid userId)
        {
            try
            {
                var wishlist = await _wishlistRepository.GetWishlistItems(userId);
                return Ok(wishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("deleteItem/{userId}/{productId}")]
        public async Task<IActionResult> RemoveWishlistItem(Guid userId, Guid productId)
        {
            try
            {
                await _wishlistRepository.RemoveWishlistItem(userId, productId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("deleteAllItem/{userId}")]
        public async Task<IActionResult> EmptyWishlist(Guid userId)
        {
            try
            {
                await _wishlistRepository.EmptyWishlist(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("updateWishlistStatus")]
        public async Task<IActionResult> UpdateWishlistStatus([FromBody] WishlistItem wishlist)
        {
            try
            {
                await _wishlistRepository.UpdateWishlistStatus(wishlist);
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
                return BadRequest();

            try
            {
                await _wishlistRepository.UpdateWishlistQuantity(id, wishlist.Quantity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
