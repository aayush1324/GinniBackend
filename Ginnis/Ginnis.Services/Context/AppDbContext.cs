using Ginnis.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Services.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }

       

        public DbSet<Address> Addresses { get; set; }

        
        public DbSet<ProductList> ProductLists{ get; set; }


        public DbSet<CartList> CartLists { get; set; }


        public DbSet<WishlistItem> WishlistItems { get; set; }

        public DbSet<OrderList> OrderLists { get; set; }

        public DbSet<RefundList> RefundLists { get; set; }

        public DbSet<ZipCode> ZipCodes { get; set; }

        public DbSet<Image> Images { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Address>().ToTable("Addresses");

            modelBuilder.Entity<ProductList>().ToTable("ProductLists");

            modelBuilder.Entity<CartList>().ToTable("CartLists");

            modelBuilder.Entity<WishlistItem>().ToTable("WishlistItems");

            modelBuilder.Entity<OrderList>().ToTable("OrderLists");

            modelBuilder.Entity<RefundList>().ToTable("RefundLists");

            modelBuilder.Entity<ZipCode>().ToTable("ZipCodes");

            modelBuilder.Entity<Image>().ToTable("Images");

        }
    }
}
