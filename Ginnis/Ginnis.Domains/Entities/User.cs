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

        //public string FirstName { get; set; }

        //public string LastName { get; set; } = string.Empty;

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Phone { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public bool EmailConfirmed { get; set; }

        public string EmailOTP { get; set; }

        public DateTime? EmailOTPExpiry { get; set; }

        public bool PhoneConfirmed { get; set; }

        public string PhoneOTP { get; set; }

        public DateTime? PhoneOTPExpiry { get; set; }

        public string ResetPasswordToken { get; set; }

        public DateTime ResetPasswordExpiry { get; set; }

        public string ConfirmationToken { get; set; }

        public DateTime? ConfirmationExpiry { get; set; }

        public bool Status { get; set; }

        public DateTime? LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Modified_at { get; set; }

        public DateTime? Deleted_at { get; set; }




        public ICollection<Address> Address { get; set; }

        public ICollection<Cart> Cart { get; set; }

        public ICollection<Wishlist> Wishlist { get; set; }

        public ICollection<Orders> Orders { get; set; }

        public ICollection<RazorpayPayment> RazorpayPayment { get; set; }


    }
}
