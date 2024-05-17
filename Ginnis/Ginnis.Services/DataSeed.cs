using Ginnis.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Services
{
    public static class DataSeed
    {
        public static void UserDataSeed(this ModelBuilder modelBuilder)
        {
            var users = new List<User>
            {
                new User
                 {
                 Id = Guid.NewGuid(),
                    UserName = "user1",
                    Email = "user1@example.com",
                    Password = "password123",
                    ConfirmPassword = "password123",
                    Phone = "1234567890",
                    Role = "User",
                    Token = "token123",
                    EmailConfirmed = true,
                    EmailOTP = "emailotp123",
                    EmailOTPExpiry = DateTime.Now.AddDays(1),
                    PhoneConfirmed = true,
                    PhoneOTP = "phoneotp123",
                    PhoneOTPExpiry = DateTime.Now.AddDays(1),
                    ResetPasswordToken = "resettoken123",
                    ResetPasswordExpiry = DateTime.Now.AddDays(1),
                    ConfirmationToken = "confirmationtoken123",
                    ConfirmationExpiry = DateTime.Now.AddDays(1),
                    Status = true,
                    LoginTime = DateTime.Now,
                    LogoutTime = DateTime.Now,
                    isDeleted = false,
                    Created_at = DateTime.Now,
                    Modified_at = DateTime.Now,
                    Deleted_at = null

                 },
            };

            modelBuilder.Entity<User>().HasData(users);
        }
    }
}
