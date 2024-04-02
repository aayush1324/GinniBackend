using Ginnis.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface IUserAuthRepo
    {
        Signup AddUser(Signup user);

        string Login(Signin login);

        Role AddRole(Role role);

        bool AssignRoleToUser(AddUserRole obj);

    }
}
