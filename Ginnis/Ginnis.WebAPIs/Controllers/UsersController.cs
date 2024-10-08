﻿using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Repos.Repositories;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailRepo _emailRepo;
        private readonly IConfirmEmailRepo _confirmEmailRepo;
        private readonly IUserRepository _userRepository;


        public UsersController(IConfiguration configuration, IEmailRepo emailRepo, IConfirmEmailRepo confirmEmailRepo,IUserRepository userRepository)
        {
            _configuration = configuration;
            _emailRepo = emailRepo;
            _confirmEmailRepo = confirmEmailRepo;
            _userRepository = userRepository;
        }



        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }

            return await _userRepository.AddUser(userObj);
        }



        [HttpPost("registerGoogle")]
        public async Task<IActionResult> GoogleAddUser([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest("Invalid request: Email is required.");
            }

            // Add the user to the database
            var addUserResult = await _userRepository.GoogleAddUser(userObj);

            if (addUserResult.Message != "User Added!")
            {
                // Return a BadRequest with the result's message
                return BadRequest(new { Message = addUserResult.Message });
            }


            var authResult = await _userRepository.GoogleAuthenticate(userObj.Email);

            if (authResult == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new
            {
                Message = addUserResult.Message,
                Token = addUserResult.Token
            }); 

        }


        [HttpPost("verifyOtps")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerify request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var message = await _userRepository.VerifyOtp(request);
           
            return Ok(new { Message = message });
        }



        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }

            return await _userRepository.Authenticate(userObj);
        }


        [HttpPost("authenticateGoogle")]
        public async Task<IActionResult> GoogleAuthenticate([FromBody] GoogleEmailDTO googleEmailDTO)
        {
            if (googleEmailDTO == null || string.IsNullOrEmpty(googleEmailDTO.Email))
            {
                return BadRequest("Invalid request: Email is required.");
            }

            var result = await _userRepository.GoogleAuthenticate(googleEmailDTO.Email);

            if (result == null)
            {
                return NotFound("User not found.");
            }

            return Ok(result); // Assuming result contains the necessary user data or token
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<string>> Logout( string token)
        {
            if (token == null || string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Message = "Invalid request" });
            }

            var message = await _userRepository.LogoutUser(token);
            return Ok(new { message });
        }


        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            try
            {
                await _userRepository.SendResetPasswordEmailAsync(email);

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Email Sent Successfully!"
                });
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = ex.Message
                });
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                await _userRepository.ResetPasswordAsync(resetPasswordDto);
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Password Reset Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getCustomer")]
        public async Task<IActionResult> GetCustomer()
        {
            return await _userRepository.GetCustomers();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddCustomer([FromForm] CustomerAddDTO customer)
        {
            return await _userRepository.AddCustomer(customer);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteCustomer/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            return await _userRepository.DeleteCustomer(customerId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("editCustomer/{customerId}")]
        public async Task<IActionResult> EditCustomer(Guid customerId, [FromForm] CustomerEditDTO updatedCustomer)
        {
            return await _userRepository.EditCustomer(customerId, updatedCustomer);
        }

    }
}
