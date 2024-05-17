using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
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






        [HttpGet("getCustomer")]
        public async Task<IActionResult> GetCustomer()
        {
            return await _userRepository.GetCustomers();
        }


        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] User customer)
        {
            return await _userRepository.AddCustomer(customer);
        }


        [HttpDelete("deleteCustomer/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            return await _userRepository.DeleteCustomer(customerId);
        }
    }
}
