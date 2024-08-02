﻿// <auto-generated />
using System;
using Ginnis.Services.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ginnis.Services.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240802061157_inttofloat")]
    partial class inttofloat
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ginnis.Domains.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Pincode")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("ItemQuantity")
                        .HasColumnType("int");

                    b.Property<int>("ItemTotalPrice")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("isPaymentDone")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Carts", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.CartList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfileImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("isPaymentDone")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("CartLists", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ProfileImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Images", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.OrderList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductAmount")
                        .HasColumnType("int");

                    b.Property<int>("ProductCount")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("OrderLists", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.Orders", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductCount")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Orderss", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.ProductList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliveryPrice")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("DiscountCoupon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DiscountedPrice")
                        .HasColumnType("int");

                    b.Property<string>("ImageData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfileImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subcategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnitLeft")
                        .HasColumnType("int");

                    b.Property<int>("UnitSold")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRating")
                        .HasColumnType("int");

                    b.Property<string>("Weight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ProductLists", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.RazorpayPayment", b =>
                {
                    b.Property<string>("RazorpayOrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("AmountDue")
                        .HasColumnType("int");

                    b.Property<int>("AmountPaid")
                        .HasColumnType("int");

                    b.Property<int>("Attempts")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Entity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfferId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Payload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PaymentSuccessful")
                        .HasColumnType("bit");

                    b.Property<string>("RazorpayPaymentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RazorpaySignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Receipt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RazorpayOrderId");

                    b.HasIndex("UserId");

                    b.ToTable("RazorpayPayments", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.RefundList", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("BatchId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("bigint");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Entity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Receipt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpeedProcessed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpeedRequested")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefundLists", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.TwilioVerify", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("VerificationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("TwilioVerifys", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConfirmPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ConfirmationExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConfirmationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("EmailOTP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EmailOTPExpiry")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LoginTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LogoutTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneOTP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PhoneOTPExpiry")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ResetPasswordExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResetPasswordToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("3e7cf061-3836-4c66-973a-cc9499fe6740"),
                            ConfirmPassword = "/ywXuc5Kuq+WvCk93pUNDc2JlWkySLMxkyTd56lGibD18s/7",
                            ConfirmationExpiry = new DateTime(2024, 8, 3, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5135),
                            ConfirmationToken = "MLlcspMx6qLute8YyPzed5AgBSW9/UEXU9WicE2iIDHH7UvVUNKJ5ZQDykPMgIeV5EAHJiLX/6vHCbeqDz1LVg==",
                            Created_at = new DateTime(2024, 8, 2, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5138),
                            Email = "aayushagrawal97@gmail.com",
                            EmailConfirmed = true,
                            EmailOTP = "635212",
                            EmailOTPExpiry = new DateTime(2024, 8, 3, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5105),
                            LoginTime = new DateTime(2024, 8, 2, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5136),
                            LogoutTime = new DateTime(2024, 8, 2, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5137),
                            Modified_at = new DateTime(2024, 8, 2, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5138),
                            Password = "WJ+gIjhFeAGMd/z0a8eZGdJLW3Y42Swj9+k5/W5E0+gbanYc",
                            Phone = "7877976611",
                            PhoneConfirmed = true,
                            PhoneOTP = "486192",
                            PhoneOTPExpiry = new DateTime(2024, 8, 3, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5133),
                            ResetPasswordExpiry = new DateTime(2024, 8, 3, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5134),
                            ResetPasswordToken = "NULL",
                            Role = "Admin",
                            Status = true,
                            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJkZjBiZGE2Mi1iOTMxLTQ5OGUtMDU1Yy0wOGRjNzg5YjI0OTYiLCJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoiQUFZVVNIIiwiRW1haWwiOiJhYXl1c2hhZ3Jhd2FsOTdAZ21haWwuY29tIiwibmJmIjoxNzE2MTg4NzI5LCJleHAiOjE3MTYxODkzMjksImlhdCI6MTcxNjE4ODcyOX0._Rdy6kaQSMJoH6TN0Z8anKhL6ZT2-V8hNprmakrm9R0",
                            UserName = "AAYUSH",
                            isDeleted = false
                        });
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.Wishlist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("ItemQuantity")
                        .HasColumnType("int");

                    b.Property<int>("ItemTotalPrice")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("isPaymentDone")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Wishlists", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.WishlistItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfileImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("WishlistStatus")
                        .HasColumnType("bit");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("WishlistItems", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.ZipCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deleted_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Delivery")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DivisionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("OfficeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfficeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PinCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ZipCodes", (string)null);
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.Address", b =>
                {
                    b.HasOne("Ginnis.Domains.Entities.User", "User")
                        .WithMany("Address")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.Cart", b =>
                {
                    b.HasOne("Ginnis.Domains.Entities.ProductList", "ProductList")
                        .WithMany("Cart")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ginnis.Domains.Entities.User", "User")
                        .WithMany("Cart")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductList");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.Image", b =>
                {
                    b.HasOne("Ginnis.Domains.Entities.ProductList", "ProductList")
                        .WithMany("Image")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductList");
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.Orders", b =>
                {
                    b.HasOne("Ginnis.Domains.Entities.ProductList", "ProductList")
                        .WithMany("Orders")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ginnis.Domains.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductList");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.RazorpayPayment", b =>
                {
                    b.HasOne("Ginnis.Domains.Entities.User", "User")
                        .WithMany("RazorpayPayment")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.Wishlist", b =>
                {
                    b.HasOne("Ginnis.Domains.Entities.ProductList", "ProductList")
                        .WithMany("Wishlist")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ginnis.Domains.Entities.User", "User")
                        .WithMany("Wishlist")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductList");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.ProductList", b =>
                {
                    b.Navigation("Cart");

                    b.Navigation("Image");

                    b.Navigation("Orders");

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("Ginnis.Domains.Entities.User", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Cart");

                    b.Navigation("Orders");

                    b.Navigation("RazorpayPayment");

                    b.Navigation("Wishlist");
                });
#pragma warning restore 612, 618
        }
    }
}
