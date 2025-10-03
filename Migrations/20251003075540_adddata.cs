using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankTransferService.Migrations
{
    /// <inheritdoc />
    public partial class adddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2025, 10, 3, 11, 25, 39, 864, DateTimeKind.Local).AddTicks(4484));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                column: "TransactionDate",
                value: new DateTime(2025, 10, 3, 11, 25, 39, 865, DateTimeKind.Local).AddTicks(1310));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2025, 10, 1, 11, 23, 1, 761, DateTimeKind.Local).AddTicks(1185));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                column: "TransactionDate",
                value: new DateTime(2025, 10, 2, 11, 23, 1, 761, DateTimeKind.Local).AddTicks(9101));
        }
    }
}
