using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Ginnis.WebAPIs.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Ginnis.Repos.Interfaces;
using Ginnis.Domains.DTOs;
using Twilio;
using Grpc.Core;
using Twilio.Rest.Api.V2010.Account;
using Twilio;
using Twilio.Types;
using Google;
using Ginnis.Services.Context;
using Google.Api;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailRepo _emailRepo;
        private readonly IConfirmEmailRepo _confirmEmailRepo;


        public UserController(AppDbContext context, IConfiguration configuration, IEmailRepo emailRepo, IConfirmEmailRepo confirmEmailRepo)
        {
            _authContext = context;
            _configuration = configuration;
            _emailRepo = emailRepo;
            _confirmEmailRepo = confirmEmailRepo;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _authContext.Users
                .FirstOrDefaultAsync(x => x.Email == userObj.Email);

            if (user == null)
                return NotFound(new { Message = "User not found!" });

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }

            if (!user.EmailConfirmed)
            {
                return BadRequest(new { Message = "Email is not confirmed yet" });
            }

            user.Token = CreateJwt(user);

            // Update user status to 1 (assuming 1 means active or logged in)
            user.Status = true;

            // Set the Created_at datetime
            user.LoginTime = DateTime.Now;

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return Ok(new 
            { 
                Token = user.Token,
                Message = "Login Success!" 
            });
        }

        

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Token == request.Token);

            if (user == null)
            {
                return NotFound(new { Message = "User not found!" });
            }

            // Perform additional validations if needed
            
            user.Token = null; // Invalidate the token or set it to null
            user.Status = false;

            // Set the Created_at datetime
            user.LogoutTime = DateTime.Now;

            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "Logout success!" });
        }

        public class LogoutRequest
        {
            public string Token { get; set; }
        }


        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            // check email
            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Already Exist" });

            var passMessage = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(passMessage))
                return BadRequest(new { Message = passMessage.ToString() });
            userObj.Password = PasswordHasher.HashPassword(userObj.Password);

            var confpassMessage = CheckPasswordStrength(userObj.ConfirmPassword);
            if (!string.IsNullOrEmpty(confpassMessage))
                return BadRequest(new { Message = confpassMessage.ToString() });
            userObj.ConfirmPassword = PasswordHasher.HashPassword(userObj.ConfirmPassword);

            // Map "mobile" from frontend to "Phone" in the backend
           

            userObj.Role = "User";
            userObj.Token = CreateJwt(userObj); // Generate JWT token
            userObj.ConfirmationExpiry = DateTime.Now.AddMinutes(15);

            // Set the Created_at datetime
            userObj.Created_at = DateTime.Now;

            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();

            // Send registration confirmation email
            await SendRegistrationConfirmationEmail(userObj.Email);

            // Send registration confirmation phone
            await SendRegistrationConfirmationPhone(userObj.Phone);

            return Ok(new
            {
                Status = 200,
                Message = "User Added!" ,
                Token = userObj.Token // Include the generated token in the response
            });;
        }


        private async Task SendRegistrationConfirmationEmail(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Email == email);

            if (user == null)
            {
                // Handle the case where the user doesn't exist
                // This should ideally not happen as the user should have been added before sending the confirmation email
                // You can log an error or throw an exception as appropriate
                return;
            }

            // Generate a confirmation token
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var confirmToken = Convert.ToBase64String(tokenBytes);

            // Update user with confirmation token and expiry time
            user.ConfirmationToken = confirmToken;
            user.ConfirmationExpiry = DateTime.Now.AddMinutes(15);

            // Generate a random OTP
            var otp = GenerateRandomOtp(); // Implement this method to generate a random OTP

            // Update user with the OTP
            user.EmailOTP = otp;
            user.EmailOTPExpiry = DateTime.Now.AddMinutes(5); // Set expiry time for OTP, adjust as needed


            // Save changes to the database
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            // Construct and send the confirmation email
            string from = _configuration["EmailSettings:From"];
            var emailModel = new ConfirmEmail(email, "Welcome to Ginni! Confirm Your Email", ConfirmEmailBody.EmailStringBody(email, confirmToken, otp));
            _confirmEmailRepo.SendConfirmEmail(emailModel);
        }


        private string GenerateRandomOtp()
        {
            //// Define the length of the OTP
            //int otpLength = 6; // 6 digits OTP

            //// Define the characters allowed in the OTP
            //const string allowedChars = "0123456789";

            //// Initialize a StringBuilder to store the OTP
            //StringBuilder otp = new StringBuilder();

            //// Initialize a Random object
            //Random random = new Random();

            //// Generate random characters and append them to the OTP
            //for (int i = 0; i < otpLength; i++)
            //{
            //    // Generate a random index to select a character from allowedChars
            //    int randomIndex = random.Next(0, allowedChars.Length);

            //    // Append the selected character to the OTP
            //    otp.Append(allowedChars[randomIndex]);
            //}
            //// Return the generated OTP as a string
            //return otp.ToString();


            Random random1 = new Random();
            string pinCode = (random1.Next() % 900000 + 100000).ToString();
            return pinCode;
        }


        private async Task SendRegistrationConfirmationPhone(string phone)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Phone == phone);

            if (user == null)
            {
                // Handle the case where the user doesn't exist
                // This should ideally not happen as the user should have been added before sending the confirmation email
                // You can log an error or throw an exception as appropriate
                return;
            }

            // Generate a random OTP
            var otp = GenerateRandomOtp(); // Implement this method to generate a random OTP

            // Update user with the OTP
            user.PhoneOTP = otp;
            user.PhoneOTPExpiry = DateTime.Now.AddMinutes(5); // Set expiry time for OTP, adjust as needed

            // Save changes to the database
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();


            string accountSid = "AC10e7e25e6be87abd8b6e39933c21b9f8";
            string authToken = "db5f6289eb9c4631b234bd7fa9eed643";
            string twilioNumber = "+12698154089";
            string countryCode = "+91";

            TwilioClient.Init(accountSid, authToken);

            try
            {
                string message = $"Welcome to Atul. Your authorization code is {otp}";

                var messageOptions = new CreateMessageOptions(new PhoneNumber(countryCode + user.Phone))
                {
                    From = new PhoneNumber(twilioNumber),
                    Body = message
                };

                var messageResponse = MessageResource.Create(messageOptions);


                if (messageResponse != null && !string.IsNullOrEmpty(messageResponse.Sid))
                {
                    // Save OTP verification data to the database
                    //var otpVerificationData = new TwilioVerify
                    //{
                    //    MobileNumber = verification.MobileNumber,
                    //    VerificationCode = pinCode
                    //};
                    //_authContext.TwilioVerifys.Add(otpVerificationData);
                    //_authContext.SaveChanges();

                    
                }
                else
                {
                   
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                // Log or return an appropriate response
            }
        }


        [HttpPost("verifyOtps")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerify request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            // Retrieve the user from the database based on the provided email
            //var user = await _authContext.Users.FirstOrDefaultAsync(u => (u.PhoneOTP == request.PhoneOtp && u.EmailOTP == request.EmailOtp));

            var user = await _authContext.Users.FirstOrDefaultAsync(u =>
                                        u.PhoneOTP == request.PhoneOtp &&
                                        u.EmailOTP == request.EmailOtp);

            if (user == null)
            {
                // User not found or OTPs expired, return a BadRequest response
                return BadRequest(new { Message = "Incorrect OTP" });
            }

            var userexpiry = await _authContext.Users.FirstOrDefaultAsync(u => 
                                        u.EmailOTPExpiry >= DateTime.Now &&
                                        u.PhoneOTPExpiry >= DateTime.Now);

            if (userexpiry == null)
            {
                return BadRequest(new { Message = "OTP expired" });

            }

            user.EmailConfirmed = true;
            user.PhoneConfirmed = true;

            //user.EmailOTP = "Done";
            //user.PhoneOTP = "Done";

            // Save changes to the database
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return Ok(new { Message = "OTP verification successful" });

            // Verify email OTP
            //if (user.EmailOTP != request.EmailOtp || user.EmailOTPExpiry < DateTime.Now)
            //{
            //    // Email OTP verification failed, return a BadRequest response
            //    return BadRequest(new { Message = "Invalid Email OTP" });
            //}

            // Verify mobile OTP
            //if (user.PhoneOTP != request.MobileOtp || user.PhoneOTPExpiry < DateTime.Now)
            //{
            //    // Mobile OTP verification failed, return a BadRequest response
            //    return BadRequest(new { Message = "Invalid Mobile OTP" });
            //}

            // Both OTPs are valid, update user status or perform any necessary actions
            // For example, you can mark the user as verified in the database


        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // Handle invalid or expired token
                return BadRequest("Invalid or expired confirmation token.");
            }

            // Update EmailConfirmed status
            user.EmailConfirmed = true;

            // Clear confirmation token and expiry
            //user.ConfirmationToken = null;
            // user.ConfirmationExpiry = null;

            // Save changes to the database
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Email Verified!"
            });
        }


        private Task<bool> CheckEmailExistAsync(string email)
           => _authContext.Users.AnyAsync(x => x.Email == email);

        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain special character" + Environment.NewLine);
            return sb.ToString();
        }


        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryveryveryveryveryverysceret.....");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, $"{user.UserName}"),
                new Claim("Email", user.Email), // Include user email in the token payload
                                                // Add more claims as needed
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }


        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Email == email);

            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email Doesn't Exist"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);

            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);

            string from = _configuration["EmailSettings:From"];

            var emailModel = new Email(email, "Reset Password!!", EmailBody.EmailStringBody(email, emailToken));

            _emailRepo.SendEmail(emailModel);

            _authContext.Entry(user).State = EntityState.Modified;

            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Email Sent!"
            });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");

            var user = await _authContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);

            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User Doesn't Exist"
                });
            }

            var tokenCode = user.ResetPasswordToken;

            DateTime emailTokenExpiry = user.ResetPasswordExpiry;

            if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid Reset link"
                });
            }

            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);

            _authContext.Entry(user).State = EntityState.Modified;

            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Password Reset Successfully"
            });
        }



        [HttpGet("getCustomer")]
        public async Task<IActionResult> GetCustomer()
        {
            var customerList = await _authContext.Users
                .Select(user => new CustomerDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role = user.Role,
                    PhoneConfirmed = user.PhoneConfirmed,
                    EmailConfirmed = user.EmailConfirmed,
                    Status = user.Status,
                    //Address = user.Address
                })
                .ToListAsync();

            if (customerList == null || customerList.Count == 0)
            {
                return NotFound();
            }

            return Ok(customerList);
        }



        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] User customer)
        {
            if (customer == null)
                return BadRequest();

            // check email
            if (await CheckEmailExistAsync(customer.Email))
                return BadRequest(new { Message = "Email Already Exist" });

            var passMessage = CheckPasswordStrength(customer.Password);
            if (!string.IsNullOrEmpty(passMessage))
                return BadRequest(new { Message = passMessage.ToString() });
            customer.Password = PasswordHasher.HashPassword(customer.Password);

            var confpassMessage = CheckPasswordStrength(customer.ConfirmPassword);
            if (!string.IsNullOrEmpty(confpassMessage))
                return BadRequest(new { Message = confpassMessage.ToString() });
            customer.ConfirmPassword = PasswordHasher.HashPassword(customer.ConfirmPassword);


            customer.Token = CreateJwt(customer); // Generate JWT token
            customer.ConfirmationExpiry = DateTime.Now.AddMinutes(15);

            await _authContext.Users.AddAsync(customer);
            await _authContext.SaveChangesAsync();

            // Send registration confirmation email
            await SendRegistrationConfirmationEmail(customer.Email);

            return Ok(new
            {
                Status = 200,
                Message = "User Added!",
                Token = customer.Token // Include the generated token in the response
            }); ;
        }


        [HttpDelete("deleteCustomer/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            try
            {
                var customer = await _authContext.Users.FindAsync(customerId);

                if (customer == null)
                {
                    return NotFound(); // Address not found
                }

                _authContext.Users.Remove(customer);
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Delete Customer Success!"
                }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("editProduct/{productId}")]
        public async Task<IActionResult> EditProduct(Guid productId, [FromBody] ProductList updatedProduct)
        {
            try
            {
                // Find the address by its unique identifier
                var product = await _authContext.ProductLists.FindAsync(productId);

                if (product == null)
                {
                    return NotFound(); // Address not found
                }

                // Update product properties with the new values
                product.ProductName = updatedProduct.ProductName;
                product.Url = updatedProduct.Url;
                product.Price = updatedProduct.Price;
                product.Discount = updatedProduct.Discount;
                product.DeliveryPrice = updatedProduct.DeliveryPrice;
                product.Quantity = updatedProduct.Quantity;
                product.Description = updatedProduct.Description;
                product.Category = updatedProduct.Category;
                product.Subcategory = updatedProduct.Subcategory;
                product.Weight = updatedProduct.Weight;
                product.Status = updatedProduct.Status;

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Update Product Success!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
