using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.DTOs
{
    public class OrderDetailDTO
    {
        public string OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public int ProductCount { get; set; }

        public int TotalAmount { get; set; }

        public byte[] ProfileImage { get; set; }  // Property to store image data as byte array (BLOB)

        public string ImageData { get; set; }       // Property to store Base64-encoded image data

        public int Price { get; set; }

        public string ProductName { get; set; }

    }
}
