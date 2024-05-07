using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ginnis.Services.Migrations
{
    /// <inheritdoc />
    public partial class cart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "WishlistItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted_at",
                table: "WishlistItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageData",
                table: "WishlistItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified_at",
                table: "WishlistItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "WishlistItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "WishlistItems",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "WishlistItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "CartLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted_at",
                table: "CartLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageData",
                table: "CartLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified_at",
                table: "CartLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "CartLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "CartLists",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CartLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "Deleted_at",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "Modified_at",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "CartLists");

            migrationBuilder.DropColumn(
                name: "Deleted_at",
                table: "CartLists");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "CartLists");

            migrationBuilder.DropColumn(
                name: "Modified_at",
                table: "CartLists");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CartLists");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "CartLists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartLists");
        }
    }
}
