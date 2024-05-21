using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ginnis.Services.Migrations
{
    /// <inheritdoc />
    public partial class intiials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6d2542d5-99b8-467e-8161-12a06c034e5f"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConfirmPassword", "ConfirmationExpiry", "ConfirmationToken", "Created_at", "Deleted_at", "Email", "EmailConfirmed", "EmailOTP", "EmailOTPExpiry", "LoginTime", "LogoutTime", "Modified_at", "Password", "Phone", "PhoneConfirmed", "PhoneOTP", "PhoneOTPExpiry", "ResetPasswordExpiry", "ResetPasswordToken", "Role", "Status", "Token", "UserName", "isDeleted" },
                values: new object[] { new Guid("87f4e88f-a069-4fed-aae4-3c023ce43a7c"), "/ywXuc5Kuq+WvCk93pUNDc2JlWkySLMxkyTd56lGibD18s/7", new DateTime(2024, 5, 21, 12, 39, 48, 294, DateTimeKind.Local).AddTicks(4990), "MLlcspMx6qLute8YyPzed5AgBSW9/UEXU9WicE2iIDHH7UvVUNKJ5ZQDykPMgIeV5EAHJiLX/6vHCbeqDz1LVg==", new DateTime(2024, 5, 20, 12, 39, 48, 294, DateTimeKind.Local).AddTicks(4993), null, "aayushagrawal97@gmail.com", true, "635212", new DateTime(2024, 5, 21, 12, 39, 48, 294, DateTimeKind.Local).AddTicks(4967), new DateTime(2024, 5, 20, 12, 39, 48, 294, DateTimeKind.Local).AddTicks(4991), new DateTime(2024, 5, 20, 12, 39, 48, 294, DateTimeKind.Local).AddTicks(4992), new DateTime(2024, 5, 20, 12, 39, 48, 294, DateTimeKind.Local).AddTicks(4993), "WJ+gIjhFeAGMd/z0a8eZGdJLW3Y42Swj9+k5/W5E0+gbanYc", "7877976611", true, "486192", new DateTime(2024, 5, 21, 12, 39, 48, 294, DateTimeKind.Local).AddTicks(4988), new DateTime(2024, 5, 21, 12, 39, 48, 294, DateTimeKind.Local).AddTicks(4990), "NULL", "Admin", true, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJkZjBiZGE2Mi1iOTMxLTQ5OGUtMDU1Yy0wOGRjNzg5YjI0OTYiLCJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoiQUFZVVNIIiwiRW1haWwiOiJhYXl1c2hhZ3Jhd2FsOTdAZ21haWwuY29tIiwibmJmIjoxNzE2MTg4NzI5LCJleHAiOjE3MTYxODkzMjksImlhdCI6MTcxNjE4ODcyOX0._Rdy6kaQSMJoH6TN0Z8anKhL6ZT2-V8hNprmakrm9R0", "AAYUSH", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("87f4e88f-a069-4fed-aae4-3c023ce43a7c"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConfirmPassword", "ConfirmationExpiry", "ConfirmationToken", "Created_at", "Deleted_at", "Email", "EmailConfirmed", "EmailOTP", "EmailOTPExpiry", "LoginTime", "LogoutTime", "Modified_at", "Password", "Phone", "PhoneConfirmed", "PhoneOTP", "PhoneOTPExpiry", "ResetPasswordExpiry", "ResetPasswordToken", "Role", "Status", "Token", "UserName", "isDeleted" },
                values: new object[] { new Guid("6d2542d5-99b8-467e-8161-12a06c034e5f"), "password123", new DateTime(2024, 5, 18, 17, 20, 17, 95, DateTimeKind.Local).AddTicks(4729), "confirmationtoken123", new DateTime(2024, 5, 17, 17, 20, 17, 95, DateTimeKind.Local).AddTicks(4731), null, "user1@example.com", true, "emailotp123", new DateTime(2024, 5, 18, 17, 20, 17, 95, DateTimeKind.Local).AddTicks(4709), new DateTime(2024, 5, 17, 17, 20, 17, 95, DateTimeKind.Local).AddTicks(4730), new DateTime(2024, 5, 17, 17, 20, 17, 95, DateTimeKind.Local).AddTicks(4730), new DateTime(2024, 5, 17, 17, 20, 17, 95, DateTimeKind.Local).AddTicks(4732), "password123", "1234567890", true, "phoneotp123", new DateTime(2024, 5, 18, 17, 20, 17, 95, DateTimeKind.Local).AddTicks(4727), new DateTime(2024, 5, 18, 17, 20, 17, 95, DateTimeKind.Local).AddTicks(4728), "resettoken123", "User", true, "token123", "user1", false });
        }
    }
}
