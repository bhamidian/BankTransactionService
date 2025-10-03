using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankTransferService.Migrations
{
    /// <inheritdoc />
    public partial class newdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "CardNumber", "Balance", "FailedAttempts", "HolderName", "Id", "IsActive", "Password" },
                values: new object[,]
                {
                    { "4580123456789012", 750000f, 0, "David Miller", 0, true, "2222" },
                    { "5892109988776655", 300000f, 0, "Frank Thomas", 0, true, "4444" },
                    { "6219865432109876", 1250000f, 0, "Emma Wilson", 0, true, "3333" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "4580123456789012");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "5892109988776655");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "6219865432109876");
        }
    }
}
