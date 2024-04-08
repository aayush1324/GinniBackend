﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class ProductVariant 
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public int Status { get; set; }


    }
}
