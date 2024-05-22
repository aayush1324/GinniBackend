using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class ProductList
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string Url { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public int DiscountedPrice { get; set; }

        public string DiscountCoupon { get; set; }

        public int DeliveryPrice { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Weight { get; set; }

        public string Status { get; set; }

        public byte[] ProfileImage { get; set; }  // Property to store image data as byte array (BLOB)

        public string ImageData { get; set; }       // Property to store Base64-encoded image data

        public bool isDeleted { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Modified_at { get; set; }

        public DateTime? Deleted_at { get; set; }



        public ICollection<Cart> Cart { get; set; }

        public ICollection<Wishlist> Wishlist { get; set; }

        public ICollection<Image> Image { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }

}
