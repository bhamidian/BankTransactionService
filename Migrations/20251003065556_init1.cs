using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankTransferService.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<float>(type: "real", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardNumber);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccessfull = table.Column<bool>(type: "bit", nullable: false),
                    SourceCardId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DestinationCardId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Cards_DestinationCardId",
                        column: x => x.DestinationCardId,
                        principalTable: "Cards",
                        principalColumn: "CardNumber");
                    table.ForeignKey(
                        name: "FK_Transactions_Cards_SourceCardId",
                        column: x => x.SourceCardId,
                        principalTable: "Cards",
                        principalColumn: "CardNumber");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DestinationCardId",
                table: "Transactions",
                column: "DestinationCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceCardId",
                table: "Transactions",
                column: "SourceCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
