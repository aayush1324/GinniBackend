using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class Orders
    {
        public Guid Id { get; set; }

        public string OrderId { get; set; }


        public Guid UserId { get; set; }
        public User User { get; set; }


        public Guid ProductId { get; set; }
        public ProductList ProductList { get; set; }    


        public int ProductCount { get; set; }

        public int TotalAmount { get; set; }

        public System.DateTime OrderDate { get; set; }

        public string Status { get; set; }

    }
}
