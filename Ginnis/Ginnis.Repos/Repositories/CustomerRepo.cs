using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly JwtContext _context;

        public CustomerRepo(JwtContext context)
        {
            _context = context;
        }

        public Customer AddCustomer(Customer customer)
        {
            var cust = _context.Customers.Add(customer);
            _context.SaveChanges();
            return cust.Entity;
        }

        public List<Customer> GetCustomerDetails()
        {
            var customers = _context.Customers.ToList();
            return customers;
        }
    }
}
