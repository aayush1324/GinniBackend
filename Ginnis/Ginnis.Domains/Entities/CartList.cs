using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class CartList
    {
        public Guid Id { get; set; }

        //public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int TotalPrice { get; set;}
    }
}
