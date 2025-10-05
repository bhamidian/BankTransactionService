using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankTransferService.Migrations
{
    /// <inheritdoc />
    public partial class addcard0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "CardNumber", "Balance", "FailedAttempts", "HolderName", "Id", "IsActive", "Password" },
                values: new object[] { "6219861820140522", 0f, 0, "Karim Karimi", 0, true, "2222" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "6219861820140522");
        }
    }
}
