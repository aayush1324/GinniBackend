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
        Task<IActionResult> AddCart(CartList cart);
        Task<IActionResult> AddToCart(CartList cart);
        Task<IActionResult> GetCart();
        Task<IActionResult> GetCart(Guid userId);
        Task<IActionResult> UpdateCartQuantity(Guid id, CartList cart);
        Task<IActionResult> RemoveCartItem(Guid id);
        Task<IActionResult> EmptyCart(Guid userId);
        Task<IActionResult> AddToWishlist(CartList item);
    }
}
