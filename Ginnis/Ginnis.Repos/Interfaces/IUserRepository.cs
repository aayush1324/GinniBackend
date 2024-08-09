using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface IUserRepository
    {
        Task<IActionResult> AddUser([FromBody] User userObj);

        Task<GoogleAddUserResult> GoogleAddUser([FromBody] User userObj);


        Task<string> VerifyOtp(OtpVerify request);

        Task<IActionResult> Authenticate([FromBody] User userObj);

        Task<IActionResult> GoogleAuthenticate(string email);


        Task<ActionResult<string>> LogoutUser(string token);

        Task SendResetPasswordEmailAsync(string email);

        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);


        Task<IActionResult> GetCustomers();

        Task<IActionResult> AddCustomer(User customer);

        Task<IActionResult> DeleteCustomer(Guid customerId);

    }
}
