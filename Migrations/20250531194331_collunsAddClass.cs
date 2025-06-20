using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuadraFacil_backend.Migrations
{
    /// <inheritdoc />
    public partial class collunsAddClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArenaId",
                table: "ClassArena",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArenaId",
                table: "ClassArena");
        }
    }
}
