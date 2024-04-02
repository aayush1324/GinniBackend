using Ginnis.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Services.Context
{
    public class JwtContext : DbContext
    {
        public JwtContext(DbContextOptions<JwtContext> options) : base(options)
        {

        }
        public DbSet<Signup> Signups { get; set; }

        public DbSet<Signin> Signins { get; set; }


        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<AddUserRole> AddUserRoles { get; set; }


        public DbSet<Customer> Customers { get; set; }
    }
}
