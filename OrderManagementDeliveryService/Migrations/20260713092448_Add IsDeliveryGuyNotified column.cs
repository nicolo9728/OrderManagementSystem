using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementDeliveryService.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeliveryGuyNotifiedcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeliveryGuyNotified",
                table: "Ordini",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeliveryGuyNotified",
                table: "Ordini");
        }
    }
}
