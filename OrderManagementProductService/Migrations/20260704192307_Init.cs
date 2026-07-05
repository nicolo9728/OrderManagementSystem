using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementProductService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prodotti",
                columns: table => new
                {
                    Codice = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    QuantitaDisponibile = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodotti", x => x.Codice);
                });

            migrationBuilder.CreateTable(
                name: "Acquisto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Momento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdUtente = table.Column<Guid>(type: "uuid", nullable: false),
                    IdProdotto = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantitaAcquistata = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acquisto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acquisto_Prodotti_IdProdotto",
                        column: x => x.IdProdotto,
                        principalTable: "Prodotti",
                        principalColumn: "Codice",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acquisto_IdProdotto",
                table: "Acquisto",
                column: "IdProdotto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acquisto");

            migrationBuilder.DropTable(
                name: "Prodotti");
        }
    }
}
