using Ginnis.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class CheckoutOrderDTO
    {
        public Guid ProductId { get; set; }
        
        public int ProductCount { get; set; }
        
        public int ProductAmount { get; set; }

        public int TotalAmount { get; set; }
    }
}
