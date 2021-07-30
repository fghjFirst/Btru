using Microsoft.EntityFrameworkCore.Migrations;

namespace Btru.Data.Migrations
{
    public partial class UserAndAssMod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reading",
                table: "ReadingAssignments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UniqueReads",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reading",
                table: "ReadingAssignments");

            migrationBuilder.DropColumn(
                name: "UniqueReads",
                table: "AspNetUsers");
        }
    }
}
