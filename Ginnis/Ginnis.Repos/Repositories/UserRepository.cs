using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
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
using Microsoft.Extensions.Configuration;
using Ginnis.WebAPIs.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Ginnis.Repos.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailRepo _emailRepo;
        private readonly IConfirmEmailRepo _confirmEmailRepo;


        public UserRepository(AppDbContext context, IConfiguration configuration, IEmailRepo emailRepo, IConfirmEmailRepo confirmEmailRepo)
        {
            _authContext = context;
            _configuration = configuration;
            _emailRepo = emailRepo;
            _confirmEmailRepo = confirmEmailRepo;
        }


       
        private async Task SendRegistrationConfirmationEmail(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Email == email);

            if (user == null)
            {      
                return;
            }

            // Generate a confirmation token
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var confirmToken = Convert.ToBase64String(tokenBytes);

            // Update user with confirmation token and expiry time
            user.ConfirmationToken = confirmToken;
            user.ConfirmationExpiry = DateTime.Now.AddMinutes(10);

            // Generate a random OTP
            var otp = GenerateRandomOtp(); // Implement this method to generate a random OTP

            // Update user with the OTP
            user.EmailOTP = otp;
            user.EmailOTPExpiry = DateTime.Now.AddMinutes(10); // Set expiry time for OTP, adjust as needed


            // Save changes to the database
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            // Construct and send the confirmation email
            string from = _configuration["EmailSettings:From"];
            var emailModel = new ConfirmEmail(email, "Welcome to Ginni! Confirm Your Email", ConfirmEmailBody.EmailStringBody(email, confirmToken, otp));
            
            _confirmEmailRepo.SendConfirmEmail(emailModel);
        }


        private async Task SendRegistrationConfirmationPhone(string phone)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Phone == phone);

            if (user == null)
            {
                return;
            }

            // Generate a random OTP
            var otp = GenerateRandomOtp(); // Implement this method to generate a random OTP

            // Update user with the OTP
            user.PhoneOTP = otp;
            user.PhoneOTPExpiry = DateTime.Now.AddMinutes(10); // Set expiry time for OTP, adjust as needed

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
            }
        }


        private string GenerateRandomOtp()
        {
            Random random1 = new Random();
            string pinCode = (random1.Next() % 900000 + 100000).ToString();
            return pinCode;
        }


        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryveryveryveryveryverysceret.....");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim("UserID", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, $"{user.UserName}"),
                new Claim("Email", user.Email), // Include user email in the token payload
                                                // Add more claims as needed
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }


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


        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                // Assuming OtpExpire is a DateTime field and you want to check if it is expired
                if (user.EmailOTPExpiry <= DateTime.Now && user.EmailConfirmed == false 
                    && user.PhoneOTPExpiry <=  DateTime.Now && user.PhoneConfirmed == false )
                {
                    _authContext.Users.Remove(user);
                    await _authContext.SaveChangesAsync();
                    return false;

                }
                if (user.EmailOTPExpiry >= DateTime.Now && user.EmailConfirmed == false
                    && user.PhoneOTPExpiry >= DateTime.Now && user.PhoneConfirmed == false)
                {
                    _authContext.Users.Remove(user);
                    await _authContext.SaveChangesAsync();
                    return false;

                }
                else
                {
                    return true;

                }
            }
            return false;
        }



        public async Task<IActionResult> AddUser([FromBody] User userObj)
        {
            if (userObj == null)
                return new BadRequestObjectResult("Invalid user");

            // Check if the email already exists
            if (await CheckEmailExistAsync(userObj.Email))
                return new BadRequestObjectResult("Email Already Exist");

            // Check password strength
            var passMessage = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(passMessage))
                return new BadRequestObjectResult(passMessage);

            // Hash the password
            userObj.Password = PasswordHasher.HashPassword(userObj.Password);

            // Check password confirmation strength
            var confpassMessage = CheckPasswordStrength(userObj.ConfirmPassword);
            if (!string.IsNullOrEmpty(confpassMessage))
                return new BadRequestObjectResult(confpassMessage);

            // Hash the confirmation password
            userObj.ConfirmPassword = PasswordHasher.HashPassword(userObj.ConfirmPassword);

            // Set user role and generate JWT token
            userObj.Role = "User";
            userObj.Token = CreateJwt(userObj);
            userObj.ConfirmationExpiry = DateTime.Now.AddMinutes(5);
            userObj.Created_at = DateTime.Now;

            // Add the user to the database
            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();

            // Send registration confirmation email
            await SendRegistrationConfirmationEmail(userObj.Email);

            // Send registration confirmation phone
            await SendRegistrationConfirmationPhone(userObj.Phone);

            // Schedule task to remove user if confirmation expires
            //ScheduleUserRemovalIfConfirmationExpires(userObj);

            return new OkObjectResult(new
            {
                Status = 200,
                Message = "User Added!",
                Token = userObj.Token // Include the generated token in the response
            });
        }


        public async Task<GoogleAddUserResult> GoogleAddUser([FromBody] User userObj)
        {
            // Check if the email already exists
            if (await CheckEmailExistAsync(userObj.Email))
            {
                return new GoogleAddUserResult
                {
                    Message = "Email Already Exist",
                    Status = 400
                };
            }

            // Set user role and generate JWT token
            userObj.Role = "User";
            userObj.Token = CreateJwt(userObj);
            userObj.EmailConfirmed = true;
            userObj.PhoneConfirmed = true;
            userObj.ConfirmationExpiry = DateTime.Now.AddMinutes(5);
            userObj.Created_at = DateTime.Now;

            // Add the user to the database
            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();

            return new GoogleAddUserResult
            {
                Message = "User Added!",
                Status = 200,
                Token = userObj.Token
            };
        }


        public async Task<string> VerifyOtp([FromBody] OtpVerify request)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u =>
                                      u.PhoneOTP == request.PhoneOtp &&
                                      u.EmailOTP == request.EmailOtp);

            var userExpiry = await _authContext.Users.FirstOrDefaultAsync(u =>
                                      u.PhoneOTP == request.PhoneOtp &&
                                      u.EmailOTP == request.EmailOtp &&
                                      u.EmailOTPExpiry >= DateTime.Now &&
                                      u.PhoneOTPExpiry >= DateTime.Now);

            if (user != null)
            {
                if (userExpiry != null)
                {
                    user.EmailConfirmed = true;
                    user.PhoneConfirmed = true;

                    _authContext.Entry(user).State = EntityState.Modified;
                    await _authContext.SaveChangesAsync();

                    return "OTP verification successful";
                }
                else
                {
                    return "Expired OTP";
                }
              
            }
            else 
            {
                return "Incorrect OTP";
            }
        }



        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return new OkObjectResult(new { Message = "Not Input" });

            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Email == userObj.Email);

            if (user == null)
                return new OkObjectResult(new { Message = "User Not Found" });

            if (!user.EmailConfirmed && !user.PhoneConfirmed)
            {
                return new OkObjectResult(new { Message = "OTP is not confirmed yet" });
            }

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return new OkObjectResult(new { Message = "Password is Incorrect" });

            }

     

            user.Token = CreateJwt(user);

            // Update user status to 1 (assuming 1 means active or logged in)
            user.Status = true;

            // Set the Created_at datetime
            user.LoginTime = DateTime.Now;

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new
            {
                Token = user.Token,
                Message = "Login Success!"
            });
        }


        public async Task<IActionResult> GoogleAuthenticate(string email)
        {
            if (email == null)
                return new OkObjectResult(new { Message = "Not Input" });

            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return new OkObjectResult(new { Message = "User Not Found" });

            if (!user.EmailConfirmed && !user.PhoneConfirmed)
            {
                return new OkObjectResult(new { Message = "OTP is not confirmed yet" });
            }

            user.Token = CreateJwt(user);

            // Update user status to 1 (assuming 1 means active or logged in)
            user.Status = true;

            // Set the Created_at datetime
            user.LoginTime = DateTime.Now;

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new
            {
                Token = user.Token,
                Message = "Login Success!"
            });
        }


        public async Task<ActionResult<string>> LogoutUser( string token)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Token == token);

            if (user == null)
            {
                return new BadRequestObjectResult("User Not Found");
            }

            user.Token = null; // Invalidate the token or set it to null
            user.Status = false;

            // Set the Created_at datetime
            user.LogoutTime = DateTime.Now;

            await _authContext.SaveChangesAsync();

            return "Logout success" ;
        }


        public async Task SendResetPasswordEmailAsync(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Email == email);

            if (user == null)
            {
                throw new Exception("Email Doesn't Exist");
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);

            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);

            string from = _configuration["EmailSettings:From"];
            var emailModel = new Email(email, "Customer account password reset", EmailBody.EmailStringBody(email, emailToken));

            _emailRepo.SendEmail(emailModel);

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
        }



        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");

            var user = await _authContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);

            if (user == null)
            {
                throw new Exception("User Doesn't Exist");
            }

            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiry;

            if (tokenCode != newToken || emailTokenExpiry < DateTime.Now)
            {
                throw new Exception("Invalid Reset Link");
            }

            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            user.Modified_at = DateTime.Now;


            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
        }








        public async Task<IActionResult> GetCustomers()
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
                })
                .ToListAsync();

            if (customerList == null || customerList.Count == 0)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(customerList);
        }



        public async Task<IActionResult> AddCustomer(User customer)
        {
            if (customer == null)
                return new BadRequestResult();

            // check email
            if (await CheckEmailExistAsync(customer.Email))
                return new BadRequestObjectResult(new { Message = "Email Already Exist" });


            var passMessage = CheckPasswordStrength(customer.Password);
            if (!string.IsNullOrEmpty(passMessage))               
                return new BadRequestObjectResult(new { Message = passMessage.ToString() });
            
            customer.Password = PasswordHasher.HashPassword(customer.Password);



            var confpassMessage = CheckPasswordStrength(customer.ConfirmPassword);
            if (!string.IsNullOrEmpty(confpassMessage))
                return new BadRequestObjectResult(new { Message = confpassMessage.ToString() });
            
            customer.ConfirmPassword = PasswordHasher.HashPassword(customer.ConfirmPassword);


            customer.Token = CreateJwt(customer); // Generate JWT token
            customer.ConfirmationExpiry = DateTime.Now.AddMinutes(15);

            await _authContext.Users.AddAsync(customer);
            await _authContext.SaveChangesAsync();

            // Send registration confirmation email
            await SendRegistrationConfirmationEmail(customer.Email);

            return new OkObjectResult(new
            {
                Status = 200,
                Message = "User Added!",
                Token = customer.Token // Include the generated token in the response
            });
        }



        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            try
            {
                var customer = await _authContext.Users.FindAsync(customerId);

                if (customer == null)
                {
                    return new NotFoundResult(); // Customer not found
                }

                _authContext.Users.Remove(customer);
                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "Delete Customer Success!" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500); // Internal server error
            }
        }






    }
}
