using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementProductService.Migrations
{
    /// <inheritdoc />
    public partial class fixDomainEventRow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DomainEventRow",
                table: "DomainEventRow");

            migrationBuilder.RenameTable(
                name: "DomainEventRow",
                newName: "Eventi");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Eventi",
                table: "Eventi",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Eventi",
                table: "Eventi");

            migrationBuilder.RenameTable(
                name: "Eventi",
                newName: "DomainEventRow");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DomainEventRow",
                table: "DomainEventRow",
                column: "Id");
        }
    }
}
