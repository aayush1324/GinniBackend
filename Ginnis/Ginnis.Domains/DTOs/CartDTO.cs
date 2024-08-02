using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class CartDTO
    {

        // Properties from Cart table
        public Guid CartId { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemTotalPrice { get; set; }

        // Properties from ProductList table
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemWeight { get; set; }
        public int ItemDiscount { get; set; }
        public string ItemDiscountCoupon { get; set; }
        public int ItemDiscountedPrice { get; set; }
        public int ItemDeliveryPrice { get; set; }
        public float ItemRating { get; set; }
        public int ItemUserRating { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ImageData { get; set; }
        
    }
}
