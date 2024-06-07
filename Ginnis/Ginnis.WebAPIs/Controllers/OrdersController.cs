using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [Authorize]
        [HttpPost("createOrder/{userId}")]
        public async Task<ActionResult<string>> CreateOrder(Guid userId)
        {
            try
            {
                var orderId = await _orderRepository.CreateOrder(userId);
                return Ok(new {orderId});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize]
        [HttpPost("createOrder/{userId}/{productId}")]
        public async Task<ActionResult<string>> CreateOrder(Guid userId, Guid productId)
        {
            try
            {
                var orderId = await _orderRepository.CreateOrder(userId, productId);
                return Ok(new { orderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize]
        [HttpGet("getOrder/{userId}")]
        public async Task<ActionResult<List<OrdersDTO>>> GetOrder(Guid userId)
        {
            try
            {
                var orderList = await _orderRepository.GetOrders(userId);
                return Ok(orderList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("getOrderById/{orderId}")]
        public async Task<ActionResult<List<OrderDetailDTO>>> GetOrderByID(string orderId)
        {
            try
            {
                var orderList = await _orderRepository.GetOrderByOrderId(orderId);
                return Ok(orderList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("getOrders")]
        public async Task<ActionResult<List<OrderListDTO>>> GetOrders()
        {
            try
            {
                var orderList = await _orderRepository.GetAllOrders();
                return Ok(orderList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
