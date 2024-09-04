using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class CustomerEditDTO
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Role { get; set; }

        public bool PhoneVerify { get; set; }

        public bool EmailVerify { get; set; }

        public bool isLoggedIn { get; set; }

    }
}
