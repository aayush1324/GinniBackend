using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public string Entity { get; set; }
        public int Amount { get; set; }
        public int AmountPaid { get; set; }
        public int AmountDue { get; set; }
        public string Currency { get; set; }
        public string Receipt { get; set; }
        public string OfferId { get; set; }
        public string Status { get; set; }
        public int Attempts { get; set; }
        public List<object> Notes { get; set; }
        public long CreatedAt { get; set; }
        // New property for custom order ID
        public string OrderId { get; set; }
        public Guid UserId { get; set; }
    }
}
