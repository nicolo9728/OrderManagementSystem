using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementDeliveryService.Migrations
{
    /// <inheritdoc />
    public partial class Addindirizzo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Indirizzo",
                table: "Ordini",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Indirizzo",
                table: "Ordini");
        }
    }
}
