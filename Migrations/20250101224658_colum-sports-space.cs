using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuadraFacil_backend.Migrations
{
    /// <inheritdoc />
    public partial class columsportsspace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sports",
                table: "Spaces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_ArenaId",
                table: "Plan",
                column: "ArenaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plan_Arenas_ArenaId",
                table: "Plan",
                column: "ArenaId",
                principalTable: "Arenas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plan_Arenas_ArenaId",
                table: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_Plan_ArenaId",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "Sports",
                table: "Spaces");
        }
    }
}
