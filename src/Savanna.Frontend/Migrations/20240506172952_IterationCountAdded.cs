using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savanna.Frontend.Migrations
{
    /// <inheritdoc />
    public partial class IterationCountAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IterationCount",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IterationCount",
                table: "Games");
        }
    }
}
