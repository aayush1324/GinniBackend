using Ginnis.Domains.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class RefundList
    {
        public string Id { get; set; }
        public string Entity { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentId { get; set; }
        public string Receipt { get; set; }
        //public AcquirerData AcquirerData { get; set; }
        public long CreatedAt { get; set; }
        public string BatchId { get; set; }
        public string Status { get; set; }
        public string SpeedProcessed { get; set; }
        public string SpeedRequested { get; set; }
    }

    public class AcquirerData
    {
        public string Arn { get; set; }
    }
}
