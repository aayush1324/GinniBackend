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


            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return Ok(new 
            { 
                Token = user.Token,
                Message = "Login Success!" 
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] User userObj)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Token == userObj.Token);

            if (user == null)
            {
                // Handle case where user with provided token is not found
                return NotFound(new { Message = "User not found!" });
            }

            // Update user status to 0 (assuming 0 means inactive or logged out)
            user.Status = false;

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Logout Success!"
            });
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

            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();

            // Send registration confirmation email
            await SendRegistrationConfirmationEmail(userObj.Email);

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

            // Save changes to the database
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            // Construct and send the confirmation email
            string from = _configuration["EmailSettings:From"];
            var emailModel = new ConfirmEmail(email, "Welcome to Ginni! Confirm Your Email", ConfirmEmailBody.EmailStringBody(email, confirmToken));
            _confirmEmailRepo.SendConfirmEmail(emailModel);
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




    }
}
