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
                var cartItems = await _authContext.CartLists.Where(c => c.UserId == userId && c.isPaymentDone == false).ToListAsync();

                string orderId = "GINNI" + DateTime.UtcNow.ToString("yyMMddHHmmss") + Guid.NewGuid().ToString("N").Substring(0, 6);

                // Construct order based on cart items
                var orders = cartItems.Select(cartItem => new OrderList
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    UserId = userId,
                    ProductId = cartItem.ProductId,
                    ProductCount = cartItem.Quantity,
                    ProductName = cartItem.ProductName,
                    ProductImage = cartItem.ImageData,
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
                var cartItem = await _authContext.CartLists.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId && c.isPaymentDone == false);

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


        [HttpGet("getOrder")]
        public async Task<IActionResult> GetOrder(Guid userId)
        {
            var orderlist = await _authContext.OrderLists.Where(o => o.UserId == userId && o.Status == "Completed")
                                                        .GroupBy(o => o.OrderId) // Group by OrderId
                                                        .Select(group => group.FirstOrDefault()) // Select the first order for each OrderId
                                                        .ToListAsync(); 

            if (orderlist == null)
            {
                return NotFound();
            }

            return Ok(orderlist);
        }


        [HttpGet("getOrderById/{orderID}")]
        public async Task<IActionResult> GetOrderByID(string orderID)
        {
            var order = await _authContext.OrderLists.Where(p => p.OrderId == orderID).ToListAsync();

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }


        [HttpGet("getOrders")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _authContext.OrderLists
                                                 .Join(_authContext.Users, // Join with the Users table
                                                    order => order.UserId, // Join on OrderList.UserId
                                                    user => user.Id,  // Join on User.UserId
                                                    (order, user) => new
                                                    {
                                                        user.UserName,
                                                        user.Email,
                                                        user.Phone,
                                                        order.OrderId,
                                                        order.UserId,
                                                        order.ProductId,
                                                        order.ProductName,
                                                        order.ProductImage,
                                                        order.ProductCount,
                                                        order.ProductAmount,
                                                        order.TotalAmount,
                                                        order.OrderDate,
                                                        //User = user // Include User details
                                                    }
                                                )
                                                .Join(
                                                    _authContext.RazorpayPayments, // Join with the RazorpayPayments table
                                                    order => order.OrderId, // Join on OrderLists.OrderId
                                                    payment => payment.OrderId, // Join on RazorpayPayments.OrderId
                                                    (order, payment) => new
                                                    {
                                                        order.UserName,
                                                        order.Email,
                                                        order.Phone,
                                                        order.OrderId,
                                                        order.UserId,
                                                        order.ProductId,
                                                        order.ProductName,
                                                        order.ProductImage,
                                                        order.ProductCount,
                                                        order.ProductAmount,
                                                        order.TotalAmount,
                                                        order.OrderDate,
                                                        payment.Receipt, // Include RazorpayPayments.PaymentId
                                                        // User = order.User // Include User details from previous join
                                                    }
                                                ).ToListAsync();

                if (orders == null || orders.Count == 0)
                {
                    return NotFound();
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
