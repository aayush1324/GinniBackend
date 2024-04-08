using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class Address
    {
        public Guid Id { get; set; }

        //public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; } = string.Empty;   

        public string Phone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; } = string.Empty;

        public int Pincode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public bool Default { get; set; }

    }
}
