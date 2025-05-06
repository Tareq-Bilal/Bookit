using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryPatternWithUOW.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionPrpertyNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BookId",
                table: "Transactions",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Books_BookId",
                table: "Transactions",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Books_BookId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BookId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Transactions");
        }
    }
}
