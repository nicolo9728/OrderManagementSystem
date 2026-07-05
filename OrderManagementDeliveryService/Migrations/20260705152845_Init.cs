using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementDeliveryService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdProdotto = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUtente = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantita = table.Column<int>(type: "integer", nullable: false),
                    IdDeliveryGuyAssegnato = table.Column<Guid>(type: "uuid", nullable: true),
                    MomentoCancellazione = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MomentoConsegna = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RagioneCancellazione = table.Column<string>(type: "text", nullable: true),
                    Tipo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
