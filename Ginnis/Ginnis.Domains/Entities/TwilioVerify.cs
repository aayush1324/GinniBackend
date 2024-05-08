using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class TwilioVerify
    {
        public Guid Id { get; set; }

        public string MobileNumber { get; set; }

        public string VerificationCode { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Modified_at { get; set; }

        public DateTime? Deleted_at { get; set; }
    }
}
