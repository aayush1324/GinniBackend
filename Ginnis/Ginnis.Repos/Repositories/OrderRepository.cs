using Ginnis.Domains.DTOs;
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
 
 
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _authContext;

        public OrderRepository(AppDbContext context)
        {
            _authContext = context;
        }


        public async Task<string> CreateOrder(Guid userId)
        {
            try
            {
                var cartItems = await _authContext.Carts.Where(c => c.UserId == userId).ToListAsync();

                string orderId = "GINNI" + DateTime.UtcNow.ToString("yyMMddHHmmss") + Guid.NewGuid().ToString("N").Substring(0, 6);

                var orders = cartItems.Select(cartItem => new Orders
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    UserId = userId,
                    ProductId = cartItem.ProductId,
                    ProductCount = cartItem.ItemQuantity,
                    TotalAmount = cartItem.ItemTotalPrice,
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                }).ToList();

                _authContext.Orderss.AddRange(orders);
                await _authContext.SaveChangesAsync();

                return orderId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<string> CreateOrder(Guid userId, Guid productId)
        {
            try
            {
                var cartItem = await _authContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
                var wishlistItem = await _authContext.Wishlists.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

                if (cartItem == null && wishlistItem == null)
                {
                    throw new Exception("Item not found in both cart and wishlist");
                }

                string orderId = "GINNI" + DateTime.UtcNow.ToString("yyMMddHHmmss") + Guid.NewGuid().ToString("N").Substring(0, 6);

                var order = new Orders
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    UserId = userId,
                    ProductId = productId,
                    ProductCount = cartItem?.ItemQuantity ?? wishlistItem.ItemQuantity,
                    TotalAmount = cartItem?.ItemTotalPrice ?? wishlistItem.ItemTotalPrice,
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                };

                _authContext.Orderss.Add(order);
                await _authContext.SaveChangesAsync();

                return orderId;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the order", ex);
            }
        }



        public async Task<ActionResult<List<OrdersDTO>>> GetOrders(Guid userId)
        {
            try
            {
                var orderList = await _authContext.Orderss
                                            .Where(o => o.UserId == userId && o.Status == "Completed")
                                            .GroupBy(o => o.OrderId)
                                             .Select(group => new OrdersDTO
                                             {
                                                 OrderId = group.Key,
                                                 TotalAmount = group.Sum(o => o.TotalAmount),
                                                 OrderDate = group.Max(o => o.OrderDate)
                                             })
                                            .ToListAsync();

                return orderList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<ActionResult<List<OrderDetailDTO>>> GetOrderByOrderId(string orderId)
        {
            try
            {
                var orderList = await _authContext.Orderss
                                                .Where(p => p.OrderId == orderId)
                                                .Join(
                                                    _authContext.ProductLists,
                                                    order => order.ProductId,
                                                    product => product.Id,
                                                    (order, product) => new OrderDetailDTO
                                                    {
                                                        OrderId = order.OrderId,
                                                        OrderDate = order.OrderDate,
                                                        ProductCount = order.ProductCount,
                                                        TotalAmount = order.TotalAmount,
                                                        ImageData = product.ImageData,
                                                        ProfileImage = product.ProfileImage,
                                                        ProductName = product.ProductName,
                                                        Price = product.Price
                                                    }
                                                )
                                                .ToListAsync();

                return orderList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<ActionResult<List<OrderListDTO>>> GetAllOrders()
        {
            try
            {
                var orderList = await _authContext.Orderss
                    .Join(
                        _authContext.Users,
                        order => order.UserId,
                        user => user.Id,
                        (order, user) => new
                        {
                            Order = order,
                            User = user
                        }
                    )
                    .Join(
                        _authContext.RazorpayPayments,
                        combined => combined.Order.OrderId,
                        payment => payment.OrderId,
                        (combined, payment) => new OrderListDTO
                        {
                            Name = combined.User.UserName,
                            Email = combined.User.Email,
                            Mobile = combined.User.Phone,
                            OrderId = combined.Order.OrderId,
                            OrderDate = combined.Order.OrderDate,
                            TransactionId = payment.RazorpayPaymentId,
                            TotalAmount = (payment.Amount)/100
                        }
                    )
                    .GroupBy(order => order.OrderId) // Group by OrderId
                    .Select(group => group.First()) // Take only the first item from each group
                    .ToListAsync();

                return orderList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

