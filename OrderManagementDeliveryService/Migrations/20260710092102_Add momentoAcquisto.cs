using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementDeliveryService.Migrations
{
    /// <inheritdoc />
    public partial class AddmomentoAcquisto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Ordini");

            migrationBuilder.AddColumn<DateTime>(
                name: "MomentoCreazione",
                table: "Eventi",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "MomentoAcquisto",
                table: "Ordini",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ordini",
                table: "Ordini",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ordini_IdProdotto_IdUtente_MomentoAcquisto",
                table: "Ordini",
                columns: new[] { "IdProdotto", "IdUtente", "MomentoAcquisto" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ordini",
                table: "Ordini");

            migrationBuilder.DropIndex(
                name: "IX_Ordini_IdProdotto_IdUtente_MomentoAcquisto",
                table: "Ordini");

            migrationBuilder.DropColumn(
                name: "MomentoCreazione",
                table: "Eventi");

            migrationBuilder.DropColumn(
                name: "MomentoAcquisto",
                table: "Ordini");

            migrationBuilder.RenameTable(
                name: "Ordini",
                newName: "Order");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");
        }
    }
}
