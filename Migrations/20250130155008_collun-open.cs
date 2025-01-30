using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuadraFacil_backend.Migrations
{
    /// <inheritdoc />
    public partial class collunopen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Open",
                table: "ArenaHours",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Open",
                table: "ArenaHours");
        }
    }
}
