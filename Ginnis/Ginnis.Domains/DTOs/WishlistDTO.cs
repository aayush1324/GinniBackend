﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class WishlistDTO
    {
        // Properties from Wishlist table
        public Guid WishlistId { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemTotalPrice { get; set; }

        // Properties from ProductList table
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ItemPrice { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ImageData { get; set; }
    }
}
