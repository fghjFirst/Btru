using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Btru.Data.Migrations
{
    public partial class Favorite_BookAssV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadingAssignments_Books_ToReadId",
                table: "ReadingAssignments");

            migrationBuilder.RenameColumn(
                name: "ToReadId",
                table: "ReadingAssignments",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadingAssignments_ToReadId",
                table: "ReadingAssignments",
                newName: "IX_ReadingAssignments_BookId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ReadingAssignments",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FavoriteBooks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteBooks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingAssignments_UserId",
                table: "ReadingAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBooks_BookId",
                table: "FavoriteBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBooks_UserId",
                table: "FavoriteBooks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingAssignments_Books_BookId",
                table: "ReadingAssignments",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingAssignments_AspNetUsers_UserId",
                table: "ReadingAssignments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadingAssignments_Books_BookId",
                table: "ReadingAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingAssignments_AspNetUsers_UserId",
                table: "ReadingAssignments");

            migrationBuilder.DropTable(
                name: "FavoriteBooks");

            migrationBuilder.DropIndex(
                name: "IX_ReadingAssignments_UserId",
                table: "ReadingAssignments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReadingAssignments");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "ReadingAssignments",
                newName: "ToReadId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadingAssignments_BookId",
                table: "ReadingAssignments",
                newName: "IX_ReadingAssignments_ToReadId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingAssignments_Books_ToReadId",
                table: "ReadingAssignments",
                column: "ToReadId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
