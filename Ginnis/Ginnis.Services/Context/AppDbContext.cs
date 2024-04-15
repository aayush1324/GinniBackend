using Ginnis.Domains.Entities;
using Microsoft.EntityFrameworkCore;
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

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Order_Address> Order_Addresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Discount> Discounts { get; set; }


        public DbSet<ProductVariant> ProductVariants { get; set; }

        public DbSet<ProductList> ProductLists{ get; set; }


        public DbSet<CartList> CartLists { get; set; }


        public DbSet<WishlistItem> WishlistItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Address>().ToTable("Addresses");

            modelBuilder.Entity<ProductList>().ToTable("ProductLists");

            modelBuilder.Entity<CartList>().ToTable("CartLists");

        }
    }
}
