using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementProductService.Migrations
{
    /// <inheritdoc />
    public partial class CreatetableDomainEventRow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DomainEventRow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Contenuto = table.Column<string>(type: "text", nullable: false),
                    IsCompletato = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEventRow", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainEventRow");
        }
    }
}
