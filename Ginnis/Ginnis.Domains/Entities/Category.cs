using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class Category
    {
        public Guid Id { get; set; }

        public string CategoryName { get; set; }

        public string Url { get; set; }

        public int ParentCategoryId { get; set; }

        public int Status { get; set; }
    }
}
