using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Razorpay.Api;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly string razorpayKeyId = "rzp_test_NHayhA8KgRDaCx";
        private readonly string razorpayKeySecret = "GMmu5cPZbH7ryafdxLFMHF7N";

        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public PaymentController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }


        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(int amount, string orderId)
        {
            try
            {
                // Initialize Razorpay client
                RazorpayClient client = new RazorpayClient("rzp_test_NHayhA8KgRDaCx", "GMmu5cPZbH7ryafdxLFMHF7N");

                // Create order options
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", amount * 100); // Amount is in paisa
                options.Add("currency", "INR");
                options.Add("receipt", "order_rcptid_" + System.DateTime.Now.Ticks.ToString());

                // Create the order
                Order order = client.Order.Create(options);

                // Map order data to OrderDto
                OrderDTO orderDto = new OrderDTO
                {
                    Id = order["id"].ToString(),
                    Entity = order["entity"].ToString(),
                    Amount = Convert.ToInt32(order["amount"]),
                    AmountPaid = Convert.ToInt32(order["amount_paid"]),
                    AmountDue = Convert.ToInt32(order["amount_due"]),
                    Currency = order["currency"].ToString(),
                    Receipt = order["receipt"].ToString(),
                    OfferId = order["offer_id"]?.ToString(),
                    Status = order["status"].ToString(),
                    Attempts = Convert.ToInt32(order["attempts"]),
                    Notes = ((JArray)order["notes"]).ToObject<List<object>>(), // Convert JArray to List<object>
                    CreatedAt = Convert.ToInt64(order["created_at"]),
                    OrderId = orderId // Assign the custom order ID
                };

                // Save order details in the database
                var entity = new RazorpayPayment
                {
                    RazorpayOrderId = orderDto.Id,
                    Amount = orderDto.Amount,
                    Currency = orderDto.Currency,
                    Receipt = orderDto.Receipt,
                    Entity = orderDto.Entity,
                    AmountPaid = orderDto.AmountPaid,
                    AmountDue = orderDto.AmountDue,
                    OfferId = orderDto.OfferId,
                    Status = orderDto.Status,
                    Attempts = orderDto.Attempts,
                    CreatedAt = DateTimeOffset.FromUnixTimeSeconds(orderDto.CreatedAt).UtcDateTime, // Convert Unix timestamp to DateTime
                    RazorpaySignature = "", // Initialize these properties
                    RazorpayPaymentId = "",
                    PaymentSuccessful = false,
                    Payload = "",
                    OrderId = orderDto.OrderId // Assign the custom order ID
                };

                // Add the entity to the context and save changes
                _authContext.RazorpayPayments.Add(entity);
                await _authContext.SaveChangesAsync();

                // Return the OrderDto
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] JsonElement data, string OrderID)
        {
            try
            {
                var signature = data.GetProperty("razorpay_signature").GetString();
                var orderId = data.GetProperty("razorpay_order_id").GetString();
                var paymentId = data.GetProperty("razorpay_payment_id").GetString();
                var payload = orderId + "|" + paymentId;
                var secret = "GMmu5cPZbH7ryafdxLFMHF7N"; // Replace with your actual Razorpay secret key

                var generatedSignature = GenerateSignature(payload, secret);

                if (signature == generatedSignature)
                {
                    // Signature verified, process payment
                    // Update the OrderList entity in the database
                    var orderListEntity = await _authContext.RazorpayPayments.FirstOrDefaultAsync(o => o.RazorpayOrderId == orderId);
                    if (orderListEntity != null)
                    {
                        orderListEntity.RazorpaySignature = signature;
                        orderListEntity.RazorpayPaymentId = paymentId;
                        orderListEntity.PaymentSuccessful = true;
                        orderListEntity.Payload = payload;

                        // Update the order status in the OrderList table to "Completed"
                        var orderdata = await _authContext.OrderLists.Where(o => o.OrderId == OrderID).ToListAsync();

                        if (orderdata != null)
                        {
                            foreach (var order in orderdata)
                            {
                                order.Status = "Completed";
                            }
                            await _authContext.SaveChangesAsync();
                        }
                        else
                        {
                            // Handle case where order with the provided orderId is not found
                            return NotFound("Order not found");
                        }

                        // Save changes to the database
                        await _authContext.SaveChangesAsync();

                        return Ok(new { message = "Payment successful" });
                    }
                    else
                    {
                        // Order not found
                        return NotFound("Order not found");
                    }
                }
                else
                {
                    // Signature mismatch, do not process payment
                    return BadRequest("Invalid signature");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing payment: {ex.Message}");
            }
        }



        public static string GenerateSignature(string payload, string secret)
        {
            var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }



        [HttpPost("failure-payment")]
        public async Task<IActionResult> FailurePayment([FromBody] JsonElement data)
        {
            try
            {
                var orderId = data.GetProperty("razorpay_order_id").GetString();

                // Update the OrderList entity in the database
                var orderListEntity = await _authContext.RazorpayPayments.FirstOrDefaultAsync(o => o.RazorpayOrderId == orderId);
                if (orderListEntity != null)
                {
                    orderListEntity.PaymentSuccessful = false;

                    // Save changes to the database
                    await _authContext.SaveChangesAsync();

                    return Ok(new { message = "Payment marked as failed" });
                }
                else
                {
                    // Order not found
                    return NotFound("Order not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing failure payment: {ex.Message}");
            }
        }



        //[HttpPost("refund-payment")]
        //public async Task<IActionResult> RefundPayment([FromBody] JsonElement data)
        //{
        //    try
        //    {
        //        var orderId = data.GetProperty("razorpay_order_id").GetString();
        //        var paymentId = data.GetProperty("razorpay_payment_id").GetString();

        //        // Logic for refunding payment goes here
        //        // For example, you can call Razorpay API to initiate a refund

        //        // Update the OrderList entity in the database
        //        var orderListEntity = await _authContext.OrderLists.FirstOrDefaultAsync(o => o.RazorpayOrderId == orderId);
        //        if (orderListEntity != null)
        //        {
        //            // Assuming refund was successful, mark the payment as failed
        //            orderListEntity.PaymentSuccessful = false;

        //            // Save changes to the database
        //            await _authContext.SaveChangesAsync();

        //            return Ok(new { message = "Payment refunded successfully" });
        //        }
        //        else
        //        {
        //            // Order not found
        //            return NotFound("Order not found");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error processing refund payment: {ex.Message}");
        //    }
        //}




        [HttpPost("refund-payment")]
        public async Task<IActionResult> RefundOrder([FromBody] JsonElement data)
        {
            try
            {
                // Initialize Razorpay client
                RazorpayClient client = new RazorpayClient("rzp_test_NHayhA8KgRDaCx", "GMmu5cPZbH7ryafdxLFMHF7N");

                string paymentId = "pay_O0ProNZmIOLgHh";

                // Create order options
                Dictionary<string, object> refundRequest = new Dictionary<string, object>();
                refundRequest.Add("amount", 200);
                refundRequest.Add("speed", "normal");

                Dictionary<string, object> notes = new Dictionary<string, object>();
                notes.Add("notes_key_1", "Tea, Earl Grey, Hot");
                notes.Add("notes_key_2", "Tea, Earl Grey… decaf.");
                refundRequest.Add("notes", notes);
                refundRequest.Add("receipt", "Receipt No." + System.DateTime.Now.Ticks.ToString());

                Refund refund = client.Payment.Fetch(paymentId).Refund(refundRequest);

                // Map refund data to RefundDTO
                RefundDTO refundDto = new RefundDTO
                {
                    Id = refund["id"].ToString(),
                    Entity = refund["entity"].ToString(),
                    Amount = Convert.ToInt32(refund["amount"]),
                    Currency = refund["currency"].ToString(),
                    PaymentId = refund["payment_id"].ToString(),
                    Receipt = refund["receipt"].ToString(),
                    AcquirerData = JsonConvert.DeserializeObject<AcquirerDataDto>(refund["acquirer_data"].ToString()), // Deserialize JSON object into AcquirerData
                    CreatedAt = Convert.ToInt64(refund["created_at"]),
                    BatchId = refund["batch_id"].ToString(),
                    Status = refund["status"].ToString(),
                    SpeedProcessed = refund["speed_processed"].ToString(),
                    SpeedRequested = refund["speed_requested"].ToString()
                };

                // Map RefundDTO to Refund entity
                var refundlist = new RefundList
                {
                    Id = refundDto.Id,
                    Entity = refundDto.Entity,
                    Amount = refundDto.Amount,
                    Currency = refundDto.Currency,
                    PaymentId = refundDto.PaymentId,
                    Receipt = refundDto.Receipt,
                    //AcquirerData = JsonConvert.DeserializeObject<AcquirerData>(refundDto.AcquirerData.ToString()),
                    CreatedAt = refundDto.CreatedAt,
                    BatchId = refundDto.BatchId,
                    Status = refundDto.Status,
                    SpeedProcessed = refundDto.SpeedProcessed,
                    SpeedRequested = refundDto.SpeedRequested
                };

                // Add refund to DbSet and save changes
                _authContext.RefundLists.Add(refundlist);
                await _authContext.SaveChangesAsync();

                return Ok(refundDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("getOrder")]
        public async Task<IActionResult> GetOrder()
        {
            var orderlist = await _authContext.OrderLists.ToListAsync();

            if (orderlist == null || orderlist.Count == 0)
            {
                return NotFound();
            }

            return Ok(orderlist);
        }


        [HttpGet("getOrderById/{orderID}")]
        public async Task<IActionResult> GetOrderByID(string orderID)
        {
            var order = await _authContext.RazorpayPayments.FirstOrDefaultAsync(p => p.OrderId == orderID);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

    }


}
