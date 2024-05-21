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
                    UserName = "AAYUSH",
                    Email = "aayushagrawal97@gmail.com",
                    Password = "WJ+gIjhFeAGMd/z0a8eZGdJLW3Y42Swj9+k5/W5E0+gbanYc",
                    ConfirmPassword = "/ywXuc5Kuq+WvCk93pUNDc2JlWkySLMxkyTd56lGibD18s/7",
                    Phone = "7877976611",
                    Role = "Admin",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJkZjBiZGE2Mi1iOTMxLTQ5OGUtMDU1Yy0wOGRjNzg5YjI0OTYiLCJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoiQUFZVVNIIiwiRW1haWwiOiJhYXl1c2hhZ3Jhd2FsOTdAZ21haWwuY29tIiwibmJmIjoxNzE2MTg4NzI5LCJleHAiOjE3MTYxODkzMjksImlhdCI6MTcxNjE4ODcyOX0._Rdy6kaQSMJoH6TN0Z8anKhL6ZT2-V8hNprmakrm9R0",
                    EmailConfirmed = true,
                    EmailOTP = "635212",
                    EmailOTPExpiry = DateTime.Now.AddDays(1),
                    PhoneConfirmed = true,
                    PhoneOTP = "486192",
                    PhoneOTPExpiry = DateTime.Now.AddDays(1),
                    ResetPasswordToken = "NULL",
                    ResetPasswordExpiry = DateTime.Now.AddDays(1),
                    ConfirmationToken = "MLlcspMx6qLute8YyPzed5AgBSW9/UEXU9WicE2iIDHH7UvVUNKJ5ZQDykPMgIeV5EAHJiLX/6vHCbeqDz1LVg==",
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
