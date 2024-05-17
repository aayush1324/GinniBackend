using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ginnis.Services.Migrations
{
    /// <inheritdoc />
    public partial class productsssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProductLists");

            migrationBuilder.DropColumn(
                name: "InWishlist",
                table: "ProductLists");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ProductLists",
                newName: "StockQuantity");

            migrationBuilder.AddColumn<int>(
                name: "DiscountedPrice",
                table: "ProductLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "ProductLists");

            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                table: "ProductLists",
                newName: "Quantity");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ProductLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InWishlist",
                table: "ProductLists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
