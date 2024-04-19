using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class RefundDTO
    {
        public string Id { get; set; }
        public string Entity { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentId { get; set; }
        public Dictionary<string, string> Notes { get; set; }
        public string Receipt { get; set; }
        public AcquirerDataDto AcquirerData { get; set; }
        public long CreatedAt { get; set; }
        public string BatchId { get; set; }
        public string Status { get; set; }
        public string SpeedProcessed { get; set; }
        public string SpeedRequested { get; set; }
    }

    public class AcquirerDataDto
    {
        public string Arn { get; set; }
    }

}
