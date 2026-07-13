using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementDeliveryService.Migrations
{
    /// <inheritdoc />
    public partial class Removeragione : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RagioneCancellazione",
                table: "Ordini");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RagioneCancellazione",
                table: "Ordini",
                type: "text",
                nullable: true);
        }
    }
}
