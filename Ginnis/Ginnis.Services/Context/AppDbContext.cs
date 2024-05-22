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
        public DbSet<Cart> Carts { get; set; }



        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }


        public DbSet<OrderList> OrderLists { get; set; }
        public DbSet<Orders> Orderss { get; set; }


        public DbSet<RazorpayPayment> RazorpayPayments { get; set; }


        public DbSet<RefundList> RefundLists { get; set; }

        public DbSet<ZipCode> ZipCodes { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<TwilioVerify> TwilioVerifys { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Address>().ToTable("Addresses");

            modelBuilder.Entity<ProductList>().ToTable("ProductLists");

            modelBuilder.Entity<CartList>().ToTable("CartLists");
            modelBuilder.Entity<Cart>().ToTable("Carts");


            modelBuilder.Entity<WishlistItem>().ToTable("WishlistItems");
            modelBuilder.Entity<Wishlist>().ToTable("Wishlists");


            modelBuilder.Entity<OrderList>().ToTable("OrderLists");
            modelBuilder.Entity<Orders>().ToTable("Orderss");


            modelBuilder.Entity<RazorpayPayment>().ToTable("RazorpayPayments");

            modelBuilder.Entity<RefundList>().ToTable("RefundLists");

            modelBuilder.Entity<ZipCode>().ToTable("ZipCodes");

            modelBuilder.Entity<Image>().ToTable("Images");

            modelBuilder.Entity<TwilioVerify>().ToTable("TwilioVerifys");

            modelBuilder.UserDataSeed();

            // Configure relationships between User and related entities
            modelBuilder.Entity<User>()
                .HasMany(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading

            modelBuilder.Entity<User>()
                .HasMany(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading

            modelBuilder.Entity<User>()
                .HasMany(u => u.Wishlist)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading

            modelBuilder.Entity<User>()
                .HasMany(u => u.RazorpayPayment)
                .WithOne(rp => rp.User)
                .HasForeignKey(rp => rp.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading



            // Configure relationships between ProductList and related entities
            modelBuilder.Entity<ProductList>()
                .HasMany(p => p.Cart)
                .WithOne(c => c.ProductList)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading

            modelBuilder.Entity<ProductList>()
                .HasMany(p => p.Wishlist)
                .WithOne(w => w.ProductList)
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading

            modelBuilder.Entity<ProductList>()
                .HasMany(p => p.Image)
                .WithOne(i => i.ProductList)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading

            modelBuilder.Entity<ProductList>()
                .HasMany(p => p.Orders)
                .WithOne(o => o.ProductList)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading



            // Configure the relationship between RazorpayPayment and Orders
            //modelBuilder.Entity<RazorpayPayment>()
            //    .HasMany(rp => rp.Orders)
            //    .WithOne(o => o.RazorpayPayment)
            //    .HasForeignKey(o => o.OrderId)
            //    .HasPrincipalKey(o => o.OrderId)
            //    .OnDelete(DeleteBehavior.Cascade); // Enable delete cascading
        }
    }
}
