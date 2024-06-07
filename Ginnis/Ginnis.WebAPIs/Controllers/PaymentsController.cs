using Ginnis.Repos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }


        [Authorize]
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(int amount, string orderId, Guid UserID)
        {
            return await _paymentRepository.CreateOrder(amount, orderId, UserID);
        }


        [Authorize]
        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] JsonElement data, string orderID, Guid userID)
        {
            return await _paymentRepository.ConfirmPayment(data, orderID, userID);
        }


        [Authorize]
        [HttpPost("failure-payment")]
        public async Task<IActionResult> FailurePayment([FromBody] JsonElement data)
        {
            return await _paymentRepository.FailurePayment(data);
        }

        [Authorize]
        [HttpPost("refund-payment")]
        public async Task<IActionResult> RefundOrder([FromBody] JsonElement data)
        {
            return await _paymentRepository.RefundOrder(data);
        }




        //[HttpGet("getOrder")]
        //public async Task<IActionResult> GetOrder(Guid userId)
        //{
        //    return await _paymentRepository.GetOrderByUserID(userId);
        //}

        //[HttpGet("getOrderById/{orderID}")]
        //public async Task<IActionResult> GetOrderByID(string orderID)
        //{
        //    return await _paymentRepository.GetOrderByID(orderID);
        //}
    }
}
