using Microsoft.EntityFrameworkCore.Migrations;

namespace Btru.Data.Migrations
{
    public partial class ReadingAssv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Read",
                table: "ReadingAssignments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Read",
                table: "ReadingAssignments");
        }
    }
}
