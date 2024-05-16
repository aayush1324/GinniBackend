﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class OrderListDTO
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string TransactionId { get; set; }

        public string OrderId { get; set; }

        public int TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }



    }
}
