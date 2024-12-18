using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuadraFacil_backend.Migrations
{
    /// <inheritdoc />
    public partial class plantable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanSelect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArenaId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plan");
        }
    }
}
