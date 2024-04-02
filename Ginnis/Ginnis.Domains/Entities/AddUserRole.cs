using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class AddUserRole
    {
        public Guid Id { get; set; } 

        public Guid UserId { get; set; }

        public List<Guid> RoleIds { get; set; }
    }
}
