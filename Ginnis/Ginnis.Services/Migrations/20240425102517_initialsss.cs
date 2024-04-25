using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ginnis.Services.Migrations
{
    /// <inheritdoc />
    public partial class initialsss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zipCode",
                table: "ZipCodes");

            migrationBuilder.AddColumn<string>(
                name: "Delivery",
                table: "ZipCodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "ZipCodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "ZipCodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeName",
                table: "ZipCodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeType",
                table: "ZipCodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "ZipCodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "ZipCodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "ZipCodes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "ZipCodes");

            migrationBuilder.DropColumn(
                name: "District",
                table: "ZipCodes");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "ZipCodes");

            migrationBuilder.DropColumn(
                name: "OfficeName",
                table: "ZipCodes");

            migrationBuilder.DropColumn(
                name: "OfficeType",
                table: "ZipCodes");

            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "ZipCodes");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "ZipCodes");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ZipCodes");

            migrationBuilder.AddColumn<string>(
                name: "zipCode",
                table: "ZipCodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
