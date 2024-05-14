using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
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


        public async Task EmptyWishlist(Guid userId)
        {
            var wishlistItems = await _authContext.WishlistItems
                .Where(item => item.UserId == userId)
                .ToListAsync();

            _authContext.WishlistItems.RemoveRange(wishlistItems);
            await _authContext.SaveChangesAsync();
        }


        public async Task<List<WishlistItem>> GetWishlistItems(Guid userId)
        {
            return await _authContext.WishlistItems
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }


        public async Task RemoveWishlistItem(Guid userId, Guid productId)
        {
            var wishlistItem = await _authContext.WishlistItems.FirstOrDefaultAsync(wi => wi.UserId == userId && wi.ProductId == productId);
            if (wishlistItem != null)
            {
                _authContext.WishlistItems.Remove(wishlistItem);     
                await _authContext.SaveChangesAsync();
            }
        }


        public async Task UpdateWishlistQuantity(Guid id, int quantity)
        {
            var existingWishlistItem = await _authContext.WishlistItems.FindAsync(id);
            if (existingWishlistItem != null)
            {
                existingWishlistItem.Quantity = quantity;
                existingWishlistItem.TotalPrice = existingWishlistItem.Price * quantity;
                await _authContext.SaveChangesAsync();
            }
        }


        public async Task UpdateWishlistStatus(WishlistItem wishlist)
        {
            var existingWishlist = await _authContext.WishlistItems.FindAsync(wishlist.Id);
            if (existingWishlist != null)
            {
                existingWishlist.WishlistStatus = wishlist.WishlistStatus;
                await _authContext.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> AddWishlistItem(WishlistItem wishlist)
        
         {
                if (wishlist == null)
                    return new BadRequestObjectResult("Wishlist item cannot be null.");

                var existingWishlistItem = await _authContext.WishlistItems.FirstOrDefaultAsync(c => c.ProductId == wishlist.ProductId && c.UserId == wishlist.UserId);

                if (existingWishlistItem != null)
                    return new ConflictObjectResult("Item already exists in the wishlist.");

                wishlist.Quantity = 1;
                wishlist.TotalPrice = wishlist.Price;
                wishlist.WishlistStatus = true;
                wishlist.Created_at = DateTime.Now;

                try
                {
                    await _authContext.WishlistItems.AddAsync(wishlist);
                    await _authContext.SaveChangesAsync();
                    return new OkObjectResult(new { Message = "Item added to Wishlist successfully" });
                }
                catch (Exception ex)
                {
                    return new StatusCodeResult(500);
                }
            }
        
    }
}
