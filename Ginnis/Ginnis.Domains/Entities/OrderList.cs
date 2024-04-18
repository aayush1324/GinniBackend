using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class OrderList
    {
        public Guid Id { get; set; }

        public decimal OrderAmount { get; set; }

        //public int OrderAmountInSubUnits
        //{
        //    get
        //    {
        //        return OrderAmount * 100;
        //    }
        //}

        public string Currency { get; set; }

        public int Payment_Capture { get; set; }

        //public Dictionary<string, string> Notes { get; set; }

        public DateTime CreatedAt { get; set; } // Optionally, include a timestamp for when the order was created
    }
}
