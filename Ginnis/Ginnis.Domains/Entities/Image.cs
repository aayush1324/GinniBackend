using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class Image
    {
        public Guid Id { get; set; }

        //public Guid Product_id { get; set; }

        public string Url { get; set; }

        public byte[] ProfileImage { get; set; }  // Property to store image data as byte array (BLOB)

        public string ImageData { get; set; }       // Property to store Base64-encoded image data
    }
}
