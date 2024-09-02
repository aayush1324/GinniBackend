using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ginnis.Services.Migrations
{
    /// <inheritdoc />
    public partial class product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("833673e1-4752-47a9-9054-23111e6848f0"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConfirmPassword", "ConfirmationExpiry", "ConfirmationToken", "Created_at", "Deleted_at", "Email", "EmailConfirmed", "EmailOTP", "EmailOTPExpiry", "LoginTime", "LogoutTime", "Modified_at", "Password", "Phone", "PhoneConfirmed", "PhoneOTP", "PhoneOTPExpiry", "ResetPasswordExpiry", "ResetPasswordToken", "Role", "Status", "Token", "UserName", "isDeleted" },
                values: new object[] { new Guid("5a136bb9-8181-4214-8bff-07ed5cb15290"), "/ywXuc5Kuq+WvCk93pUNDc2JlWkySLMxkyTd56lGibD18s/7", new DateTime(2024, 9, 3, 14, 51, 54, 710, DateTimeKind.Local).AddTicks(7553), "MLlcspMx6qLute8YyPzed5AgBSW9/UEXU9WicE2iIDHH7UvVUNKJ5ZQDykPMgIeV5EAHJiLX/6vHCbeqDz1LVg==", new DateTime(2024, 9, 2, 14, 51, 54, 710, DateTimeKind.Local).AddTicks(7557), null, "aayushagrawal97@gmail.com", true, "635212", new DateTime(2024, 9, 3, 14, 51, 54, 710, DateTimeKind.Local).AddTicks(7527), new DateTime(2024, 9, 2, 14, 51, 54, 710, DateTimeKind.Local).AddTicks(7554), new DateTime(2024, 9, 2, 14, 51, 54, 710, DateTimeKind.Local).AddTicks(7556), new DateTime(2024, 9, 2, 14, 51, 54, 710, DateTimeKind.Local).AddTicks(7558), "WJ+gIjhFeAGMd/z0a8eZGdJLW3Y42Swj9+k5/W5E0+gbanYc", "7877976611", true, "486192", new DateTime(2024, 9, 3, 14, 51, 54, 710, DateTimeKind.Local).AddTicks(7551), new DateTime(2024, 9, 3, 14, 51, 54, 710, DateTimeKind.Local).AddTicks(7552), "NULL", "Admin", true, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJkZjBiZGE2Mi1iOTMxLTQ5OGUtMDU1Yy0wOGRjNzg5YjI0OTYiLCJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoiQUFZVVNIIiwiRW1haWwiOiJhYXl1c2hhZ3Jhd2FsOTdAZ21haWwuY29tIiwibmJmIjoxNzE2MTg4NzI5LCJleHAiOjE3MTYxODkzMjksImlhdCI6MTcxNjE4ODcyOX0._Rdy6kaQSMJoH6TN0Z8anKhL6ZT2-V8hNprmakrm9R0", "AAYUSH", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5a136bb9-8181-4214-8bff-07ed5cb15290"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConfirmPassword", "ConfirmationExpiry", "ConfirmationToken", "Created_at", "Deleted_at", "Email", "EmailConfirmed", "EmailOTP", "EmailOTPExpiry", "LoginTime", "LogoutTime", "Modified_at", "Password", "Phone", "PhoneConfirmed", "PhoneOTP", "PhoneOTPExpiry", "ResetPasswordExpiry", "ResetPasswordToken", "Role", "Status", "Token", "UserName", "isDeleted" },
                values: new object[] { new Guid("833673e1-4752-47a9-9054-23111e6848f0"), "/ywXuc5Kuq+WvCk93pUNDc2JlWkySLMxkyTd56lGibD18s/7", new DateTime(2024, 9, 3, 14, 50, 43, 790, DateTimeKind.Local).AddTicks(1604), "MLlcspMx6qLute8YyPzed5AgBSW9/UEXU9WicE2iIDHH7UvVUNKJ5ZQDykPMgIeV5EAHJiLX/6vHCbeqDz1LVg==", new DateTime(2024, 9, 2, 14, 50, 43, 790, DateTimeKind.Local).AddTicks(1609), null, "aayushagrawal97@gmail.com", true, "635212", new DateTime(2024, 9, 3, 14, 50, 43, 790, DateTimeKind.Local).AddTicks(1582), new DateTime(2024, 9, 2, 14, 50, 43, 790, DateTimeKind.Local).AddTicks(1606), new DateTime(2024, 9, 2, 14, 50, 43, 790, DateTimeKind.Local).AddTicks(1607), new DateTime(2024, 9, 2, 14, 50, 43, 790, DateTimeKind.Local).AddTicks(1610), "WJ+gIjhFeAGMd/z0a8eZGdJLW3Y42Swj9+k5/W5E0+gbanYc", "7877976611", true, "486192", new DateTime(2024, 9, 3, 14, 50, 43, 790, DateTimeKind.Local).AddTicks(1602), new DateTime(2024, 9, 3, 14, 50, 43, 790, DateTimeKind.Local).AddTicks(1603), "NULL", "Admin", true, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJkZjBiZGE2Mi1iOTMxLTQ5OGUtMDU1Yy0wOGRjNzg5YjI0OTYiLCJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoiQUFZVVNIIiwiRW1haWwiOiJhYXl1c2hhZ3Jhd2FsOTdAZ21haWwuY29tIiwibmJmIjoxNzE2MTg4NzI5LCJleHAiOjE3MTYxODkzMjksImlhdCI6MTcxNjE4ODcyOX0._Rdy6kaQSMJoH6TN0Z8anKhL6ZT2-V8hNprmakrm9R0", "AAYUSH", false });
        }
    }
}
