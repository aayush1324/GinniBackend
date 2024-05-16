using Ginnis.Domains.DTOs;
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
        Task<IActionResult> AddWishlistItem(Guid userId, Guid productId);

        Task<IEnumerable<WishlistDTO>> GetWishlistItems(Guid userId);

        Task<IActionResult> RemoveWishlistItem(Guid userId, Guid productId);

        Task<IActionResult> EmptyWishlist(Guid userId);

        Task<IActionResult> UpdateWishlistQuantity(WishlistDTO wishlist);

    }
}
