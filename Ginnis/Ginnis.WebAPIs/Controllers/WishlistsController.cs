using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Repos.Repositories;
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




        [HttpPost("addWishlist/{userId}/{productId}")]
        public async Task<IActionResult> AddWishlist(Guid userId, Guid productId)
        {
            return await _wishlistRepository.AddWishlistItem(userId, productId);
        }



        [HttpGet("getWishlists/{userId}")]
        public async Task<IActionResult> GetWishlist(Guid userId)
        {
            try
            {
                var wishlistDTOs = await _wishlistRepository.GetWishlistItems(userId);
                return Ok(wishlistDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpDelete("deleteItem/{userId}/{productId}")]
        public async Task<IActionResult> RemoveWishlistItem(Guid userId, Guid productId)
        {
            return await _wishlistRepository.RemoveWishlistItem(userId, productId);
        }



        [HttpDelete("deleteAllItem/{userId}")]
        public async Task<IActionResult> EmptyWishlist(Guid userId)
        {
            return await _wishlistRepository.EmptyWishlist(userId);
        }



        [HttpPut("updateWishlistQuantity")]
        public async Task<IActionResult> UpdateWishlistQuantity([FromBody] WishlistDTO wishlist)
        {
            return await _wishlistRepository.UpdateWishlistQuantity(wishlist);

        }
      
    }
}
