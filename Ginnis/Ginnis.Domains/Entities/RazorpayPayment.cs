using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class RazorpayPayment
    {
        [Key]
        public string RazorpayOrderId { get; set; }


        [Required]
        public int Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Receipt { get; set; }

        public string Entity { get; set; }

        public int AmountPaid { get; set; }

        public int AmountDue { get; set; }

        public string OfferId { get; set; }

        public string Status { get; set; }

        public int Attempts { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; } // For concurrency control

        public DateTime CreatedAt { get; set; }


        // New properties

        public string RazorpaySignature { get; set; }

        public string RazorpayPaymentId { get; set; }

        public bool PaymentSuccessful { get; set; }

        public string Payload { get; set; }

        // New property for custom order ID
        public string OrderId { get; set; }

        public Guid UserId { get; set; }

    }
}
