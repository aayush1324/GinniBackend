using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public OrderController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }


        // OrderController.cs

        [HttpPost("createOrder/{userId}")]
        public async Task<ActionResult<OrderList>> CreateOrder(Guid userId)
        {
            try
            {
                // Fetch cart items for the given userId from your Cartlists table
                var cartItems = await _authContext.CartLists.Where(c => c.UserId == userId).ToListAsync();

                string orderId = "GINNI" + DateTime.UtcNow.ToString("yyMMddHHmmss") + Guid.NewGuid().ToString("N").Substring(0, 6);

                // Construct order based on cart items
                var orders = cartItems.Select(cartItem => new OrderList
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    UserId = userId,
                    ProductId = cartItem.ProductId,
                    ProductCount = cartItem.Quantity,
                    ProductAmount = cartItem.Price,
                    TotalAmount = cartItem.Quantity * cartItem.Price,
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                }).ToList();

                // Save the orders to the OrderLists table
                _authContext.OrderLists.AddRange(orders);
                await _authContext.SaveChangesAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("createOrder/{userId}/{productId}")]
        public async Task<ActionResult<OrderList>> CreateOrder(Guid userId, Guid productId)
        {
            try
            {
                // Fetch cart items for the given userId and productId from your Cartlists table
                var cartItem = await _authContext.CartLists.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

                if (cartItem == null)
                {
                    return NotFound("Cart item not found");
                }

                string orderId = "GINNI" + DateTime.UtcNow.ToString("yyMMddHHmmss") + Guid.NewGuid().ToString("N").Substring(0, 6);

                // Construct order based on cart item
                var order = new OrderList
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    UserId = userId,
                    ProductId = productId,
                    ProductCount = cartItem.Quantity,
                    ProductAmount = cartItem.Price,
                    TotalAmount = cartItem.Quantity * cartItem.Price,
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                };

                // Save the order to the OrderLists table
                _authContext.OrderLists.Add(order);
                await _authContext.SaveChangesAsync();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
