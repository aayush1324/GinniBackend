using Ginnis.Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface IWishlistRepository
    {
        Task<IActionResult> AddWishlistItem(WishlistItem wishlist);

        Task<List<WishlistItem>> GetWishlistItems(Guid userId);

        Task RemoveWishlistItem(Guid userId, Guid productId);

        Task EmptyWishlist(Guid userId);

        Task UpdateWishlistStatus(WishlistItem wishlist);

        Task UpdateWishlistQuantity(Guid id, int quantity);

 
    }
}
