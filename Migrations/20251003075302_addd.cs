using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankTransferService.Migrations
{
    /// <inheritdoc />
    public partial class addd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedAttempts",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "CardNumber", "Balance", "FailedAttempts", "HolderName", "Id", "IsActive", "Password" },
                values: new object[,]
                {
                    { "5022291111222233", 2500000f, 0, "Charlie Brown", 0, true, "1111" },
                    { "6037991234567890", 1500000f, 0, "Alice Johnson", 0, true, "1234" },
                    { "6274120987654321", 500000f, 0, "Bob Smith", 0, true, "9999" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "DestinationCardId", "IsSuccessfull", "SourceCardId", "TransactionDate" },
                values: new object[,]
                {
                    { 1, 200000f, "6274120987654321", true, "6037991234567890", new DateTime(2025, 10, 1, 11, 23, 1, 761, DateTimeKind.Local).AddTicks(1185) },
                    { 2, 150000f, "5022291111222233", true, "6274120987654321", new DateTime(2025, 10, 2, 11, 23, 1, 761, DateTimeKind.Local).AddTicks(9101) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "5022291111222233");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "6037991234567890");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "6274120987654321");

            migrationBuilder.DropColumn(
                name: "FailedAttempts",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Cards");
        }
    }
}
