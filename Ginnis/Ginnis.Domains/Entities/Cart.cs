using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }


        public Guid UserId { get; set; }
        public User User { get; set; }


        public Guid ProductId { get; set; }
        public ProductList ProductList { get; set; }


        public int ItemQuantity { get; set; }

        public int ItemTotalPrice { get; set; }

        public bool isPaymentDone { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Modified_at { get; set; }

        public DateTime? Deleted_at { get; set; }
    }

}
