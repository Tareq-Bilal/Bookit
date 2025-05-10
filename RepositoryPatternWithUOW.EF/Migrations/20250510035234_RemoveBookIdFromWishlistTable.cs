using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryPatternWithUOW.EF.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookIdFromWishlistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key if it exists
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Books_BookId",
                table: "Wishlists");

            // Drop index if it exists
            migrationBuilder.DropIndex(
                name: "IX_Wishlists_BookId",
                table: "Wishlists");

            // Drop BookId column
            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Wishlists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add BookId column back
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Wishlists",
                type: "int",
                nullable: true);

            // Create index
            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_BookId",
                table: "Wishlists",
                column: "BookId");

            // Add foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Books_BookId",
                table: "Wishlists",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
