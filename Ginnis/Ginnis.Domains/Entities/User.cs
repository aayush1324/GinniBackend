using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        //public bool EmailConfirmed { get; set; }

        //public bool PhoneConfirmed { get; set; }

        //public bool Status { get; set; }

        public string ResetPasswordToken { get; set; }

        public DateTime ResetPasswordExpiry { get; set; }

        public string ConfirmationToken { get; set; }

        public DateTime ConfirmationExpiry { get; set; }

        public bool IsEmailConfirmed { get; set; }
    }
}
