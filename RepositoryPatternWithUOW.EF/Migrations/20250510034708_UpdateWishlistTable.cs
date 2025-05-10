using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryPatternWithUOW.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWishlistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Authors_AuthorId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_AuthorId",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Wishlists");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wishlists",
                type: "nvarchar(100)",  // Changed from nvarchar(max) to nvarchar(100)
                maxLength: 100,         // Added explicit max length
                nullable: true);        // Changed from not nullable to nullable
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wishlists");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Wishlists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_AuthorId",
                table: "Wishlists",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Authors_AuthorId",
                table: "Wishlists",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
