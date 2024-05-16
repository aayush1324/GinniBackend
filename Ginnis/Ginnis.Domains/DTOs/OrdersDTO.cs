using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class OrdersDTO
    {
        public string OrderId { get; set; }

        public int TotalAmount { get; set; }
        
        public DateTime OrderDate { get; set; }
    }
}
