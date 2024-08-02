using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ginnis.Services.Migrations
{
    /// <inheritdoc />
    public partial class inttofloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("74913cee-8d78-48e1-a3c0-c3be9ccd61cf"));

            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                table: "ProductLists",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConfirmPassword", "ConfirmationExpiry", "ConfirmationToken", "Created_at", "Deleted_at", "Email", "EmailConfirmed", "EmailOTP", "EmailOTPExpiry", "LoginTime", "LogoutTime", "Modified_at", "Password", "Phone", "PhoneConfirmed", "PhoneOTP", "PhoneOTPExpiry", "ResetPasswordExpiry", "ResetPasswordToken", "Role", "Status", "Token", "UserName", "isDeleted" },
                values: new object[] { new Guid("3e7cf061-3836-4c66-973a-cc9499fe6740"), "/ywXuc5Kuq+WvCk93pUNDc2JlWkySLMxkyTd56lGibD18s/7", new DateTime(2024, 8, 3, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5135), "MLlcspMx6qLute8YyPzed5AgBSW9/UEXU9WicE2iIDHH7UvVUNKJ5ZQDykPMgIeV5EAHJiLX/6vHCbeqDz1LVg==", new DateTime(2024, 8, 2, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5138), null, "aayushagrawal97@gmail.com", true, "635212", new DateTime(2024, 8, 3, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5105), new DateTime(2024, 8, 2, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5136), new DateTime(2024, 8, 2, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5137), new DateTime(2024, 8, 2, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5138), "WJ+gIjhFeAGMd/z0a8eZGdJLW3Y42Swj9+k5/W5E0+gbanYc", "7877976611", true, "486192", new DateTime(2024, 8, 3, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5133), new DateTime(2024, 8, 3, 11, 41, 56, 692, DateTimeKind.Local).AddTicks(5134), "NULL", "Admin", true, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJkZjBiZGE2Mi1iOTMxLTQ5OGUtMDU1Yy0wOGRjNzg5YjI0OTYiLCJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoiQUFZVVNIIiwiRW1haWwiOiJhYXl1c2hhZ3Jhd2FsOTdAZ21haWwuY29tIiwibmJmIjoxNzE2MTg4NzI5LCJleHAiOjE3MTYxODkzMjksImlhdCI6MTcxNjE4ODcyOX0._Rdy6kaQSMJoH6TN0Z8anKhL6ZT2-V8hNprmakrm9R0", "AAYUSH", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3e7cf061-3836-4c66-973a-cc9499fe6740"));

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "ProductLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConfirmPassword", "ConfirmationExpiry", "ConfirmationToken", "Created_at", "Deleted_at", "Email", "EmailConfirmed", "EmailOTP", "EmailOTPExpiry", "LoginTime", "LogoutTime", "Modified_at", "Password", "Phone", "PhoneConfirmed", "PhoneOTP", "PhoneOTPExpiry", "ResetPasswordExpiry", "ResetPasswordToken", "Role", "Status", "Token", "UserName", "isDeleted" },
                values: new object[] { new Guid("74913cee-8d78-48e1-a3c0-c3be9ccd61cf"), "/ywXuc5Kuq+WvCk93pUNDc2JlWkySLMxkyTd56lGibD18s/7", new DateTime(2024, 8, 3, 10, 59, 31, 61, DateTimeKind.Local).AddTicks(6876), "MLlcspMx6qLute8YyPzed5AgBSW9/UEXU9WicE2iIDHH7UvVUNKJ5ZQDykPMgIeV5EAHJiLX/6vHCbeqDz1LVg==", new DateTime(2024, 8, 2, 10, 59, 31, 61, DateTimeKind.Local).AddTicks(6880), null, "aayushagrawal97@gmail.com", true, "635212", new DateTime(2024, 8, 3, 10, 59, 31, 61, DateTimeKind.Local).AddTicks(6852), new DateTime(2024, 8, 2, 10, 59, 31, 61, DateTimeKind.Local).AddTicks(6877), new DateTime(2024, 8, 2, 10, 59, 31, 61, DateTimeKind.Local).AddTicks(6878), new DateTime(2024, 8, 2, 10, 59, 31, 61, DateTimeKind.Local).AddTicks(6880), "WJ+gIjhFeAGMd/z0a8eZGdJLW3Y42Swj9+k5/W5E0+gbanYc", "7877976611", true, "486192", new DateTime(2024, 8, 3, 10, 59, 31, 61, DateTimeKind.Local).AddTicks(6874), new DateTime(2024, 8, 3, 10, 59, 31, 61, DateTimeKind.Local).AddTicks(6875), "NULL", "Admin", true, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJkZjBiZGE2Mi1iOTMxLTQ5OGUtMDU1Yy0wOGRjNzg5YjI0OTYiLCJyb2xlIjoiVXNlciIsInVuaXF1ZV9uYW1lIjoiQUFZVVNIIiwiRW1haWwiOiJhYXl1c2hhZ3Jhd2FsOTdAZ21haWwuY29tIiwibmJmIjoxNzE2MTg4NzI5LCJleHAiOjE3MTYxODkzMjksImlhdCI6MTcxNjE4ODcyOX0._Rdy6kaQSMJoH6TN0Z8anKhL6ZT2-V8hNprmakrm9R0", "AAYUSH", false });
        }
    }
}
