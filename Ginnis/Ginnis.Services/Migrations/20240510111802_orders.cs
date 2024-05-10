using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ginnis.Services.Migrations
{
    /// <inheritdoc />
    public partial class orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPaymentDone",
                table: "CartLists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPaymentDone",
                table: "CartLists");
        }
    }
}
