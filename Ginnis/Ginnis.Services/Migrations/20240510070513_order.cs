using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ginnis.Services.Migrations
{
    /// <inheritdoc />
    public partial class order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckoutOrderDTO",
                table: "OrderLists");

            migrationBuilder.AddColumn<int>(
                name: "ProductAmount",
                table: "OrderLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductCount",
                table: "OrderLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "OrderLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TotalAmount",
                table: "OrderLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductAmount",
                table: "OrderLists");

            migrationBuilder.DropColumn(
                name: "ProductCount",
                table: "OrderLists");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderLists");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "OrderLists");

            migrationBuilder.AddColumn<string>(
                name: "CheckoutOrderDTO",
                table: "OrderLists",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
