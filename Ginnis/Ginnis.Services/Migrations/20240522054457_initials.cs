using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ginnis.Services.Migrations
{
    /// <inheritdoc />
    public partial class initials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orderss_RazorpayPayments_RazorpayPaymentRazorpayOrderId",
                table: "Orderss");

            migrationBuilder.DropIndex(
                name: "IX_Orderss_RazorpayPaymentRazorpayOrderId",
                table: "Orderss");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("09725f93-d529-4d85-af8a-a98d820b6257"));

            migrationBuilder.DropColumn(
                name: "RazorpayPaymentRazorpayOrderId",
                table: "Orderss");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConfirmPassword", "ConfirmationExpiry", "ConfirmationToken", "Created_at", "Deleted_at", "Email", "EmailConfirmed", "EmailOTP", "EmailOTPExpiry", "LoginTime", "LogoutTime", "Modified_at", "Password", "Phone", "PhoneConfirmed", "PhoneOTP", "PhoneOTPExpiry", "ResetPasswordExpiry", "ResetPasswordToken", "Role", "Status", "Token", "UserName", "isDeleted" },
                values: new object[] { new Guid("2071ef38-b733-4ba2-a1bb-b46a0616a06c"), "/ywXuc5Kuq+WvCk93pUNDc2JlWkySLMxkyTd56lGibD18s/7", new DateTime(2024, 5, 23, 11, 14, 57, 85, DateTimeKind.Local).AddTicks(3900), "MLlcspMx6qLute8YyPzed5AgBSW9/UEXU9WicE2iIDHH7UvVUNKJ5ZQDykPMgIeV5EAHJiLX/6vHCbeqDz1LVg==", new DateTime(2024, 5, 22, 11, 14, 57, 85, DateTimeKind.Local).AddTicks(3902), null, "aayushagrawal97@gmail.com", true, "635212", new DateTime(2024, 5, 23, 11, 14, 57, 85, DateTimeKind.Local).AddTicks(3880), new DateTime(2024, 5, 22, 11, 14, 57, 85, DateTimeKind.Local).AddTicks(3901), new DateTime(2024, 5, 22, 11, 14, 57, 85, DateTimeKind.Local).AddTicks(3902), new DateTime(2024, 5, 22, 11, 14, 57, 85, DateTimeKind.Local).AddTicks(3903), "WJ+gIjhFeAGMd/z0a8eZGdJLW3Y42Swj9+k5/W5E0+gbanYc", "7877976611", true, "486192", new DateTime(2024, 5, 23, 11, 14, 57, 85, DateTimeKind.Local).AddTicks(3899), new DateTime(2024, 5, 23, 11, 14, 57, 85, DateTimeKind.Local).AddTicks(3900), "NULL", "Admin", true, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJkZjBiZGE2Mi1iOTMxLTQ5OGUtMDU1Yy0wOGRjNzg5YjI0OTYiLCJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoiQUFZVVNIIiwiRW1haWwiOiJhYXl1c2hhZ3Jhd2FsOTdAZ21haWwuY29tIiwibmJmIjoxNzE2MTg4NzI5LCJleHAiOjE3MTYxODkzMjksImlhdCI6MTcxNjE4ODcyOX0._Rdy6kaQSMJoH6TN0Z8anKhL6ZT2-V8hNprmakrm9R0", "AAYUSH", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2071ef38-b733-4ba2-a1bb-b46a0616a06c"));

            migrationBuilder.AddColumn<string>(
                name: "RazorpayPaymentRazorpayOrderId",
                table: "Orderss",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConfirmPassword", "ConfirmationExpiry", "ConfirmationToken", "Created_at", "Deleted_at", "Email", "EmailConfirmed", "EmailOTP", "EmailOTPExpiry", "LoginTime", "LogoutTime", "Modified_at", "Password", "Phone", "PhoneConfirmed", "PhoneOTP", "PhoneOTPExpiry", "ResetPasswordExpiry", "ResetPasswordToken", "Role", "Status", "Token", "UserName", "isDeleted" },
                values: new object[] { new Guid("09725f93-d529-4d85-af8a-a98d820b6257"), "/ywXuc5Kuq+WvCk93pUNDc2JlWkySLMxkyTd56lGibD18s/7", new DateTime(2024, 5, 23, 11, 11, 5, 201, DateTimeKind.Local).AddTicks(2541), "MLlcspMx6qLute8YyPzed5AgBSW9/UEXU9WicE2iIDHH7UvVUNKJ5ZQDykPMgIeV5EAHJiLX/6vHCbeqDz1LVg==", new DateTime(2024, 5, 22, 11, 11, 5, 201, DateTimeKind.Local).AddTicks(2543), null, "aayushagrawal97@gmail.com", true, "635212", new DateTime(2024, 5, 23, 11, 11, 5, 201, DateTimeKind.Local).AddTicks(2523), new DateTime(2024, 5, 22, 11, 11, 5, 201, DateTimeKind.Local).AddTicks(2542), new DateTime(2024, 5, 22, 11, 11, 5, 201, DateTimeKind.Local).AddTicks(2542), new DateTime(2024, 5, 22, 11, 11, 5, 201, DateTimeKind.Local).AddTicks(2544), "WJ+gIjhFeAGMd/z0a8eZGdJLW3Y42Swj9+k5/W5E0+gbanYc", "7877976611", true, "486192", new DateTime(2024, 5, 23, 11, 11, 5, 201, DateTimeKind.Local).AddTicks(2539), new DateTime(2024, 5, 23, 11, 11, 5, 201, DateTimeKind.Local).AddTicks(2540), "NULL", "Admin", true, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJkZjBiZGE2Mi1iOTMxLTQ5OGUtMDU1Yy0wOGRjNzg5YjI0OTYiLCJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoiQUFZVVNIIiwiRW1haWwiOiJhYXl1c2hhZ3Jhd2FsOTdAZ21haWwuY29tIiwibmJmIjoxNzE2MTg4NzI5LCJleHAiOjE3MTYxODkzMjksImlhdCI6MTcxNjE4ODcyOX0._Rdy6kaQSMJoH6TN0Z8anKhL6ZT2-V8hNprmakrm9R0", "AAYUSH", false });

            migrationBuilder.CreateIndex(
                name: "IX_Orderss_RazorpayPaymentRazorpayOrderId",
                table: "Orderss",
                column: "RazorpayPaymentRazorpayOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orderss_RazorpayPayments_RazorpayPaymentRazorpayOrderId",
                table: "Orderss",
                column: "RazorpayPaymentRazorpayOrderId",
                principalTable: "RazorpayPayments",
                principalColumn: "RazorpayOrderId");
        }
    }
}
