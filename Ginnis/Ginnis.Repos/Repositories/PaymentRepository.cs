using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Ginnis.Repos.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string razorpayKeyId = "rzp_test_NHayhA8KgRDaCx";
        private readonly string razorpayKeySecret = "GMmu5cPZbH7ryafdxLFMHF7N";

        private readonly AppDbContext _authContext;

        public PaymentRepository(AppDbContext context)
        {
            _authContext = context;
        }


        public async Task<IActionResult> CreateOrder(int amount, string orderId, Guid UserID)
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
                    OrderId = orderId, // Assign the custom order ID
                    UserId = UserID
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
                    OrderId = orderDto.OrderId, // Assign the custom order ID
                    UserId = orderDto.UserId
                };

                // Add the entity to the context and save changes
                _authContext.RazorpayPayments.Add(entity);
                await _authContext.SaveChangesAsync();

                // Return the OrderDto
                return new OkObjectResult(orderDto);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        private static string GenerateSignature(string payload, string secret)
        {
            var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }


        public async Task<IActionResult> ConfirmPayment(JsonElement data, string OrderID, Guid UserID)
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
                    // Update the RazorpayPayment entity in the database
                    var orderEntity = await _authContext.RazorpayPayments.FirstOrDefaultAsync(o => o.RazorpayOrderId == orderId);
                    if (orderEntity != null)
                    {
                        orderEntity.RazorpaySignature = signature;
                        orderEntity.RazorpayPaymentId = paymentId;
                        orderEntity.PaymentSuccessful = true;
                        orderEntity.Payload = payload;

                        // Update the order status in the database to "Completed"
                        var orders = await _authContext.Orderss.Where(o => o.OrderId == OrderID && o.Status == "Pending").ToListAsync();
                        if (orders != null)
                        {
                            foreach (var order in orders)
                            {
                                order.Status = "Completed";
                            }
                            await _authContext.SaveChangesAsync();
                        }
                        else
                        {
                            // Handle case where order with the provided orderId is not found
                            return new NotFoundObjectResult("Order not found");
                        }

                        // Remove cart items for the specified user
                        var cartItems = await _authContext.Carts.Where(c => c.UserId == UserID && !c.isPaymentDone).ToListAsync();
                        _authContext.Carts.RemoveRange(cartItems);

                        // Save changes to the database
                        await _authContext.SaveChangesAsync();

                        return new OkObjectResult(new { message = "Payment successful" });
                    }
                    else
                    {
                        // Order not found
                        return new NotFoundObjectResult("Order not found");
                    }
                }
                else
                {
                    // Signature mismatch, do not process payment
                    return new BadRequestObjectResult("Invalid signature");
                }
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        public async Task<IActionResult> FailurePayment(JsonElement data)
        {
            try
            {
                var orderId = data.GetProperty("razorpay_order_id").GetString();

                // Update the RazorpayPayment entity in the database
                var orderEntity = await _authContext.RazorpayPayments.FirstOrDefaultAsync(o => o.RazorpayOrderId == orderId);
                if (orderEntity != null)
                {
                    orderEntity.PaymentSuccessful = false;

                    // Save changes to the database
                    await _authContext.SaveChangesAsync();

                    return new OkObjectResult(new { message = "Payment marked as failed" });
                }
                else
                {
                    // Order not found
                    return new NotFoundObjectResult("Order not found");
                }
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        public async Task<IActionResult> RefundOrder(JsonElement data)
        {
            try
            {
                RazorpayClient client = new RazorpayClient(razorpayKeyId, razorpayKeySecret);

                string paymentId = "pay_O0ProNZmIOLgHh";

                Dictionary<string, object> refundRequest = new Dictionary<string, object>();
                refundRequest.Add("amount", 200);
                refundRequest.Add("speed", "normal");

                Dictionary<string, object> notes = new Dictionary<string, object>();
                notes.Add("notes_key_1", "Tea, Earl Grey, Hot");
                notes.Add("notes_key_2", "Tea, Earl Grey… decaf.");
                refundRequest.Add("notes", notes);
                refundRequest.Add("receipt", "Receipt No." + DateTime.Now.Ticks.ToString());

                Refund refund = client.Payment.Fetch(paymentId).Refund(refundRequest);

                RefundDTO refundDto = new RefundDTO
                {
                    Id = refund["id"].ToString(),
                    Entity = refund["entity"].ToString(),
                    Amount = Convert.ToInt32(refund["amount"]),
                    Currency = refund["currency"].ToString(),
                    PaymentId = refund["payment_id"].ToString(),
                    Receipt = refund["receipt"].ToString(),
                    AcquirerData = JsonSerializer.Deserialize<AcquirerDataDto>(refund["acquirer_data"].ToString()),
                    CreatedAt = Convert.ToInt64(refund["created_at"]),
                    BatchId = refund["batch_id"].ToString(),
                    Status = refund["status"].ToString(),
                    SpeedProcessed = refund["speed_processed"].ToString(),
                    SpeedRequested = refund["speed_requested"].ToString()
                };

                var refundlist = new RefundList
                {
                    Id = refundDto.Id,
                    Entity = refundDto.Entity,
                    Amount = refundDto.Amount,
                    Currency = refundDto.Currency,
                    PaymentId = refundDto.PaymentId,
                    Receipt = refundDto.Receipt,
                    CreatedAt = refundDto.CreatedAt,
                    BatchId = refundDto.BatchId,
                    Status = refundDto.Status,
                    SpeedProcessed = refundDto.SpeedProcessed,
                    SpeedRequested = refundDto.SpeedRequested
                };

                _authContext.RefundLists.Add(refundlist);
                await _authContext.SaveChangesAsync();

                return new OkObjectResult(refundDto);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }




        //public async Task<List<RazorpayPayment>> GetOrderByUserID(Guid userId)
        //{
        //    var orderlist = await _authContext.RazorpayPayments.Where(o => o.UserId == userId && o.PaymentSuccessful == true).ToListAsync();
        //    return orderlist;
        //}

        //public async Task<RazorpayPayment> GetOrderByID(string orderID)
        //{
        //    var order = await _authContext.RazorpayPayments.FirstOrDefaultAsync(p => p.OrderId == orderID);
        //    return order;
        //}


    }
}
