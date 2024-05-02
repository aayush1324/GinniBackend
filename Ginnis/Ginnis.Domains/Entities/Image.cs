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
    }
}
