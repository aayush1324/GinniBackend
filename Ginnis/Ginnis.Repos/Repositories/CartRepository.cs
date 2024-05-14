using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _authContext;

        public CartRepository(AppDbContext authContext)
        {
            _authContext = authContext;
        }


        public async Task<IActionResult> AddCart(CartList cart)
        {
            if (cart == null)
                return new BadRequestResult();

            try
            {
                var existingCartItem = await _authContext.CartLists.FirstOrDefaultAsync(c => c.ProductName == cart.ProductName);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += 1;
                    existingCartItem.TotalPrice = existingCartItem.Quantity * existingCartItem.Price;
                }
                else
                {
                    cart.Quantity = 1;
                    cart.TotalPrice = cart.Price;
                    await _authContext.CartLists.AddAsync(cart);
                }

                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "Item added to cart successfully" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        public async Task<IActionResult> AddToCart(CartList cart)
        {
            if (cart == null)
                return new BadRequestObjectResult("Invalid cart data.");

            try
            {
                var existingCartItem = await _authContext.CartLists.FirstOrDefaultAsync(c => c.ProductId == cart.ProductId && c.UserId == cart.UserId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += cart.Quantity;
                    existingCartItem.TotalPrice = existingCartItem.Quantity * existingCartItem.Price;
                }
                else
                {
                    await _authContext.CartLists.AddAsync(cart);
                }

                cart.Created_at = DateTime.Now;
                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "Item added to cart successfully" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        public async Task<IActionResult> AddToWishlist(CartList item)
        {
            if (item == null)
                return new BadRequestResult();

            try
            {
                var existingCartItem = await _authContext.CartLists.FirstOrDefaultAsync(c => c.ProductName == item.ProductName);

                if (existingCartItem != null)
                {
                    _authContext.CartLists.Remove(existingCartItem);
                    await _authContext.SaveChangesAsync();
                }

                var wishlistItem = new WishlistItem
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.TotalPrice,
                    // Set other properties as needed
                };

                await _authContext.WishlistItems.AddAsync(wishlistItem);
                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "Item added to wishlist successfully" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    




        public async Task<IActionResult> EmptyCart(Guid userId)
            {
                var cartItems = await _authContext.CartLists
                    .Where(item => item.UserId == userId)
                    .ToListAsync();

                _authContext.CartLists.RemoveRange(cartItems);
                await _authContext.SaveChangesAsync();

                return new NoContentResult();
            }



        public async Task<IActionResult> GetCart()
        {
            try
            {
                var cartList = await _authContext.CartLists.ToListAsync();

                if (cartList == null || cartList.Count == 0)
                    return new NotFoundResult();

                return new OkObjectResult(cartList);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        public async Task<IActionResult> GetCart(Guid userId)
        {
            try
            {
                var cartList = await _authContext.CartLists
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                return new OkObjectResult(cartList);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        public async Task<IActionResult> RemoveCartItem(Guid id)
        {
            var cartItem = await _authContext.CartLists.FindAsync(id);

            if (cartItem == null)
                return new NotFoundResult();

            _authContext.CartLists.Remove(cartItem);
            await _authContext.SaveChangesAsync();

            return new NoContentResult();
        }



        public async Task<IActionResult> UpdateCartQuantity(Guid id, CartList cart)
        {
            if (id != cart.Id)
                return new BadRequestResult();

            var existingCartItem = await _authContext.CartLists.FindAsync(id);

            if (existingCartItem == null)
                return new NotFoundResult();

            existingCartItem.Quantity = cart.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * existingCartItem.Price;

            _authContext.Entry(existingCartItem).State = EntityState.Modified;

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
