using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class CustomerAddDTO
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Role { get; set; }

        public bool PhoneVerify { get; set; }

        public bool EmailVerify { get; set; }

        public bool isLoggedIn { get; set; }

        public string Token { get; set; }


    }
}
