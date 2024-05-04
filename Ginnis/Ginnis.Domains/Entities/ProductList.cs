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

        public int DeliveryPrice { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Weight { get; set; }

        public string Status { get; set; }

        public string Image { get; set; }


        //public IFormFile Image { get; set; }

        //public byte[] Images { get; set; }

        public bool CartStatus { get; set; }

        public bool WishlistStatus { get; set; }
    }
}
