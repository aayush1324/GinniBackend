using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class OrderListDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }
    
        public string OrderId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public string ProductPrice { get; set; }

        public string ProductCount { get; set; }

        public string ProductAmount { get; set; }

        public string TotalAmount { get; set; }
    
    }
}
