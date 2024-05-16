using Ginnis.Domains.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Twilio.Rest.Api.V2010.Account;
using Twilio;
using Twilio.Types;
using Google;
using Ginnis.Services.Context;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public TwilioController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("VerificationCall")]
        public IActionResult VerificationCall(TwilioVerify verification)
        {
            try
            {
                string accountSid = "AC10e7e25e6be87abd8b6e39933c21b9f8";
                string authToken = "db5f6289eb9c4631b234bd7fa9eed643";
                string twilioNumber = "+12698154089";
                string countryCode = "+91";

                TwilioClient.Init(accountSid, authToken);

                if (string.IsNullOrEmpty(verification.MobileNumber))
                {
                    return NotFound("Wrong number");
                }

                Regex regex = new Regex(@"^[0-9]*$");
                string mobileno = string.Empty;

                if (verification.MobileNumber.Contains("-"))
                {
                    string[] number = verification.MobileNumber.Split('-');
                    mobileno = number[1];
                }

                Match m = regex.Match(verification.MobileNumber);
                verification.VerificationCode = string.Empty;
                Random random = new Random();
                string pinCode = (random.Next() % 90000 + 10000).ToString();
                verification.VerificationCode = pinCode.ToString();

                string message = $"Welcome%20to%20Atul%20Your%20authorization%20code%20is%20{Uri.EscapeDataString(pinCode.ToString())}&";
                string content = $"http://twimlets.com/message?Message%5B0%5D={message}";

                var options = new CreateCallOptions(new PhoneNumber(countryCode + verification.MobileNumber), new PhoneNumber(twilioNumber))
                {
                    Url = new Uri(content),
                    Method = Twilio.Http.HttpMethod.Get
                };

                var call = CallResource.Create(options);

                if (call != null)
                {
                    return Ok("Please wait for call for your verification code");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        [Route("VerificationOtp")]
        public IActionResult VerificationOtp(TwilioVerify verification)
        {
            try
            {
                string accountSid = _configuration["Twilio:AccountSid"];
                string authToken = _configuration["Twilio:AuthToken"];
                string twilioNumber = _configuration["Twilio:PhoneNumber"];
                string countryCode = _configuration["Twilio:CountryCode"];

                TwilioClient.Init(accountSid, authToken);

                if (string.IsNullOrEmpty(verification.MobileNumber))
                {
                    return NotFound("Wrong number");
                }

                Regex regex = new Regex(@"^[0-9]*$");
                string mobileno = string.Empty;

                if (verification.MobileNumber.Contains("-"))
                {
                    string[] number = verification.MobileNumber.Split('-');
                    mobileno = number[1];
                }

                Match m = regex.Match(verification.MobileNumber);

                verification.VerificationCode = string.Empty;
                Random random = new Random();
                string pinCode = (random.Next() % 900000 + 100000).ToString();
                verification.VerificationCode = pinCode.ToString();

                string message = $"Welcome to Ginni Dry Fruits. Your authorization code is {pinCode}";

                var messageOptions = new CreateMessageOptions(new PhoneNumber(countryCode + verification.MobileNumber))
                {
                    From = new PhoneNumber(twilioNumber),
                    Body = message
                };

                var messageResponse = MessageResource.Create(messageOptions);

                if (messageResponse != null && !string.IsNullOrEmpty(messageResponse.Sid))
                {
                    // Save OTP verification data to the database
                    var otpVerificationData = new TwilioVerify
                    {
                        MobileNumber = verification.MobileNumber,
                        VerificationCode = pinCode
                    };
                    _authContext.TwilioVerifys.Add(otpVerificationData);
                    _authContext.SaveChanges();

                    return Ok("SMS sent successfully");
                }
                else
                {
                    return NotFound("Failed to send SMS");
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("VerifyOtp")]
        public IActionResult VerifyOtp(string mobileNumber, string verificationCode)
        {
            try
            {
                // Check if both mobile number and verification code are provided
                if (string.IsNullOrEmpty(mobileNumber) || string.IsNullOrEmpty(verificationCode))
                {
                    return BadRequest("Mobile number and verification code are required.");
                }

                // Retrieve the verification data from the database based on the mobile number
                var otpVerificationData = _authContext.TwilioVerifys.SingleOrDefault(v => v.MobileNumber == mobileNumber);

                // Check if verification data exists for the provided mobile number
                if (otpVerificationData == null)
                {
                    return NotFound("No verification data found for the provided mobile number.");
                }

                // Check if the verification code provided by the user matches the one stored in the database
                if (otpVerificationData.VerificationCode != verificationCode)
                {
                    return BadRequest("Invalid verification code.");
                }

                // Verification successful, remove only the verification code
                otpVerificationData.VerificationCode = null;
                _authContext.SaveChanges();

                return Ok("Verification successful");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}
