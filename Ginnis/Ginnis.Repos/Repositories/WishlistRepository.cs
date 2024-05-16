using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Ginnis.Services.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _authContext;

        public WishlistRepository(AppDbContext context)
        {
            _authContext = context;
        }


        public async Task<IActionResult> AddWishlistItem(Guid userId, Guid productId)

        {
            try
            {
                var existingWishlistItem = await _authContext.Wishlists.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

                // Find the product in the ProductLists table by productId
                var product = await _authContext.ProductLists.FirstOrDefaultAsync(p => p.Id == productId);

                // Assuming you have a property named 'Price' in the ProductLists table
                var itemPrice = product.Price;

                if (existingWishlistItem != null)
                {
                    return new ConflictObjectResult("Item already exists in the wishlist.");
                }
                else
                {
                    var wishlistItem = new Wishlist
                    {
                        UserId = userId,
                        ProductId = productId,
                        ItemQuantity = 1, // Assuming default quantity
                        ItemTotalPrice = itemPrice * 1,
                        Created_at = DateTime.Now
                    };

                    await _authContext.Wishlists.AddAsync(wishlistItem);
                }

                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "Item added to Wishlist successfully" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        public async Task<IEnumerable<WishlistDTO>> GetWishlistItems(Guid userId)
        {
            return await _authContext.Wishlists
                                    .Where(c => c.UserId == userId)
                                    .Join(
                                            _authContext.ProductLists,
                                            wishlist => wishlist.ProductId,
                                            product => product.Id,
                                            (wishlist, product) => new WishlistDTO
                                            {
                                                WishlistId = wishlist.Id,
                                                ItemQuantity = wishlist.ItemQuantity,
                                                ItemTotalPrice = wishlist.ItemTotalPrice,
                                                ProductId = product.Id,
                                                ProductName = product.ProductName,
                                                ItemPrice = product.Price,
                                                ProfileImage = product.ProfileImage,
                                                ImageData = product.ImageData
                                            })
                                    .ToListAsync();
        }



        public async Task<IActionResult> RemoveWishlistItem(Guid userId, Guid productId)
        {
            var wishlistItem = await _authContext.Wishlists.FirstOrDefaultAsync(wi => wi.UserId == userId && wi.ProductId == productId);


            if (wishlistItem == null)
                return new NotFoundResult();
     
            _authContext.Wishlists.Remove(wishlistItem);
            await _authContext.SaveChangesAsync();
            
            return new NoContentResult();
        }



        public async Task<IActionResult> EmptyWishlist(Guid userId)
        {
            var wishlistItems = await _authContext.Wishlists
                                                .Where(item => item.UserId == userId)
                                                .ToListAsync();

            _authContext.Wishlists.RemoveRange(wishlistItems);
            await _authContext.SaveChangesAsync();

            return new NoContentResult();

        }



        public async Task<IActionResult> UpdateWishlistQuantity(WishlistDTO wishlist)
        {
            var existingWishlistItem = await _authContext.Wishlists.FindAsync(wishlist.WishlistId);

            if (existingWishlistItem == null)
                return new NotFoundResult();

            // Find the product in the ProductLists table by productId
            var product = await _authContext.ProductLists.FirstOrDefaultAsync(p => p.Id == wishlist.ProductId);

            // Assuming you have a property named 'Price' in the ProductLists table
            var itemPrice = product.Price;

            existingWishlistItem.ItemQuantity = wishlist.ItemQuantity;
            existingWishlistItem.ItemTotalPrice = existingWishlistItem.ItemQuantity * itemPrice;

            _authContext.Entry(existingWishlistItem).State = EntityState.Modified;


            try
            {
                await _authContext.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        
    }
}
