using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class Product
    {

        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string Url { get; set; }

        public Guid CategoryId { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }    

        public int Quantity { get; set; }

        public int Status { get; set; }
    }
}
