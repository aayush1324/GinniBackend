﻿using Ginnis.Domains.DTOs;
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

        Task<string> VerifyOtp(OtpVerify request);

        Task<IActionResult> Authenticate([FromBody] User userObj);

        Task<ActionResult<string>> LogoutUser(string token);

        Task<IActionResult> GetCustomers();

        Task<IActionResult> AddCustomer(User customer);

        Task<IActionResult> DeleteCustomer(Guid customerId);

    }
}
