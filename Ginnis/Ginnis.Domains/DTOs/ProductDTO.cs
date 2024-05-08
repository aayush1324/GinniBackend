﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string Url { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public string DiscountCoupon { get; set; }

        public int DeliveryPrice { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Weight { get; set; }

        public string Status { get; set; }

        public string Image { get; set; }                    //  not use

        public byte[] ProfileImage { get; set; }  // Property to store image data as byte array (BLOB)

        public string ImageData { get; set; }       // Property to store Base64-encoded image data

        public bool isDeleted { get; set; }


        // New property to indicate whether the product is in the wishlist
        public bool InWishlist { get; set; }

        public bool InCart { get; set; }
    }

}
