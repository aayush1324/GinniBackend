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
    public interface ICartRepository
    {
        Task<IActionResult> AddToCart(Guid userId, Guid productId);

        Task<IEnumerable<CartDTO>> GetCart(Guid userId);

        Task<IActionResult> RemoveCartItem(Guid userId, Guid itemId);

        Task<IActionResult> EmptyCart(Guid userId);

        Task<IActionResult> UpdateCartQuantity(CartDTO cart);





        Task<IActionResult> AddCart(CartList cart);
        Task<IActionResult> GetCart();
        Task<IActionResult> AddToWishlist(CartList item);
    }
}
