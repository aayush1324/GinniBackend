using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        //[HttpPost("create-order")]
        //public async Task<IActionResult> CreateOrder()
        //{
        //    try
        //    {
        //        // Generate your unique merchant order ID here
        //        var merchantOrderId = Guid.NewGuid().ToString(); // Example: Generate a GUID as the order ID

        //        using (var httpClient = new HttpClient())
        //        {
        //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{razorpayKeyId}:{razorpayKeySecret}")));

        //            var payload = new Dictionary<string, object>
        //    {
        //        { "amount", 20000 }, // Amount in paisa (e.g., 20000 for ₹200)
        //        { "currency", "INR" },
        //        // Add more fields as needed
        //        { "merchant_order_id", merchantOrderId } // Include the merchant order ID
        //    };

        //            var jsonPayload = JsonConvert.SerializeObject(payload);
        //            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        //            var response = await httpClient.PostAsync("https://api.razorpay.com/v1/orders", content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var responseContent = await response.Content.ReadAsStringAsync();
        //                dynamic order = JsonConvert.DeserializeObject(responseContent);
        //                return Ok(order);
        //            }
        //            else
        //            {
        //                // Handle error
        //                return StatusCode((int)response.StatusCode, "Failed to create order");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Failed to create order: {ex.Message}");
        //    }
        //}


        //[HttpPost("confirm-payment")]
        //public IActionResult ConfirmPayment([FromBody] dynamic data)
        //{
        //    // Verify signature and process payment
        //    var signature = (string)data.signature;
        //    var orderId = (string)data.razorpay_order_id;
        //    var paymentId = (string)data.razorpay_payment_id;
        //    var payload = orderId + "|" + paymentId;
        //    var secret = "GMmu5cPZbH7ryafdxLFMHF7N";

        //    var generatedSignature = GenerateSignature(payload, secret);

        //    if (signature == generatedSignature)
        //    {
        //        // Signature verified, process payment
        //        return Ok("Payment successful");
        //    }
        //    else
        //    {
        //        // Signature mismatch, do not process payment
        //        return BadRequest("Invalid signature");
        //    }
        //}

        [HttpPost("create-order")]
        public async Task<IActionResult> InitiatePayment(int amount)
        {
            try
            {
                RazorpayClient client = new RazorpayClient("rzp_test_NHayhA8KgRDaCx", "GMmu5cPZbH7ryafdxLFMHF7N");

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", amount * 100); // Amount is in paisa
                options.Add("currency", "INR");
                options.Add("receipt", "order_rcptid_" + System.DateTime.Now.Ticks.ToString());
                Order order = client.Order.Create(options);
                
                Console.WriteLine(order["amount"]);
                return Ok(order.Attributes);
             
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500); // Internal server error
            }
        }


        [HttpPost("confirm-payment")]
        public IActionResult ConfirmPayment([FromBody] JsonElement data)
        {
            try
            {
                var signature = data.GetProperty("signature").GetString();
                var orderId = data.GetProperty("razorpay_order_id").GetString();
                var paymentId = data.GetProperty("razorpay_payment_id").GetString();
                var payload = orderId + "|" + paymentId;
                var secret = "GMmu5cPZbH7ryafdxLFMHF7N"; // Replace with your actual Razorpay secret key

                var generatedSignature = GenerateSignature(payload, secret);

                if (signature == generatedSignature)
                {
                    // Signature verified, process payment
                    return Ok("Payment successful");
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
    }
}
