using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankTransferService.Migrations
{
    /// <inheritdoc />
    public partial class deletingIdforcard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Cards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "4580123456789012",
                column: "Id",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "5022291111222233",
                column: "Id",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "5892109988776655",
                column: "Id",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "6037991234567890",
                column: "Id",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "6219861820140522",
                column: "Id",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "6219865432109876",
                column: "Id",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardNumber",
                keyValue: "6274120987654321",
                column: "Id",
                value: 0);
        }
    }
}
