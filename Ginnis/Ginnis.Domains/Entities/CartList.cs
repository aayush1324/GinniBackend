using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class CartList
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int TotalPrice { get; set;}

        public byte[] ProfileImage { get; set; }  // Property to store image data as byte array (BLOB)

        public string ImageData { get; set; }       // Property to store Base64-encoded image data

        public bool isDeleted { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Modified_at { get; set; }

        public DateTime? Deleted_at { get; set; }
    }
}
