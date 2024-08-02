using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Google.Api;
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



        public async Task<IActionResult> AddToCart(Guid userId, Guid productId)
        {
            try
            {
                var existingCartItem = await _authContext.Carts.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);
               
                // Find the product in the ProductLists table by productId
                var product = await _authContext.ProductLists.FirstOrDefaultAsync(p => p.Id == productId);

                // Assuming you have a property named 'Price' in the ProductLists table
                var itemPrice = product.DiscountedPrice;
                
                if (existingCartItem != null)
                {
                    existingCartItem.ItemQuantity += 1;
                    existingCartItem.ItemTotalPrice = existingCartItem.ItemQuantity * itemPrice;
                }
                else
                {
                    var cartItem = new Cart
                    {
                        UserId = userId,
                        ProductId = productId,
                        ItemQuantity = 1, // Assuming default quantity
                        ItemTotalPrice = itemPrice * 1,
                        Created_at = DateTime.Now
                    };

                    await _authContext.Carts.AddAsync(cartItem);

                }
                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "Item added to cart successfully" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        public async Task<IEnumerable<CartDTO>> GetCart(Guid userId)
        {
            return await _authContext.Carts
                                    .Where(c => c.UserId == userId)
                                    .Join(
                                            _authContext.ProductLists,
                                            cart => cart.ProductId,
                                            product => product.Id,
                                            (cart, product) => new CartDTO
                                            {
                                                CartId = cart.Id,
                                                ItemQuantity = cart.ItemQuantity,
                                                ItemTotalPrice = cart.ItemTotalPrice,
                                                ProductId = product.Id,
                                                ProductName = product.ProductName,
                                                ItemPrice = product.Price,
                                                ItemDiscountedPrice = product.DiscountedPrice,
                                                ItemDiscount = product.Discount,
                                                ItemDeliveryPrice = product.DeliveryPrice,
                                                ItemDiscountCoupon = product.DiscountCoupon,
                                                ItemWeight = product.Weight,
                                                ItemRating = product.Rating,
                                                ItemUserRating = product.UserRating,
                                                ProfileImage = product.ProfileImage,
                                                ImageData = product.ImageData
                                            })
                                    .ToListAsync();
        }



        public async Task<IActionResult> RemoveCartItem(Guid userId, Guid itemId)
        {
            var cartItem = await _authContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == itemId);

            if (cartItem == null)
                return new NotFoundResult();

            _authContext.Carts.Remove(cartItem);
            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new { Message = "Cart item Deleted successfully" });
        }



        public async Task<IActionResult> EmptyCart(Guid userId)
        {
            var cartItems = await _authContext.Carts
                                            .Where(item => item.UserId == userId)
                                            .ToListAsync();

            _authContext.Carts.RemoveRange(cartItems);
            await _authContext.SaveChangesAsync();

            return new NoContentResult();
        }



        public async Task<IActionResult> UpdateCartQuantity(CartDTO cart)
        {
            var existingCartItem = await _authContext.Carts.FindAsync(cart.CartId);

            if (existingCartItem == null)
                return new NotFoundResult();

            // Find the product in the ProductLists table by productId
            var product = await _authContext.ProductLists.FirstOrDefaultAsync(p => p.Id == cart.ProductId);

            // Assuming you have a property named 'Price' in the ProductLists table
            var itemPrice = product.DiscountedPrice;

            existingCartItem.ItemQuantity = cart.ItemQuantity;
            existingCartItem.ItemTotalPrice = existingCartItem.ItemQuantity * itemPrice;

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
      
    }
}
