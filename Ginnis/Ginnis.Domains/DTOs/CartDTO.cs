using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class CartDTO
    {
        public Guid Id { get; set; }
        public int CartQuantity { get; set; }
        public int TotalPrice { get; set; }
        public string ProductName { get; set; }
        public string ImageData { get; set; }
    }
}
