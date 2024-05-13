using Ginnis.Domains.DTOs;
using Google.Type;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class OrderList
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public Guid UserId { get; set; }
        // Include properties from CheckoutOrderDTO
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }       // Property to store Base64-encoded image data

        public int ProductCount { get; set; }
        public int ProductAmount { get; set; }
        public int TotalAmount { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
