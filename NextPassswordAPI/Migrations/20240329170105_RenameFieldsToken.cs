using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextPassswordAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldsToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Passwords_AssociatedPasswordId",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "AssociatedPasswordId",
                table: "Tokens",
                newName: "PasswordId");

            migrationBuilder.RenameIndex(
                name: "IX_Tokens_AssociatedPasswordId",
                table: "Tokens",
                newName: "IX_Tokens_PasswordId");

            migrationBuilder.RenameColumn(
                name: "AssociatedTokenId",
                table: "Passwords",
                newName: "TokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Passwords_PasswordId",
                table: "Tokens",
                column: "PasswordId",
                principalTable: "Passwords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Passwords_PasswordId",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "PasswordId",
                table: "Tokens",
                newName: "AssociatedPasswordId");

            migrationBuilder.RenameIndex(
                name: "IX_Tokens_PasswordId",
                table: "Tokens",
                newName: "IX_Tokens_AssociatedPasswordId");

            migrationBuilder.RenameColumn(
                name: "TokenId",
                table: "Passwords",
                newName: "AssociatedTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Passwords_AssociatedPasswordId",
                table: "Tokens",
                column: "AssociatedPasswordId",
                principalTable: "Passwords",
                principalColumn: "Id");
        }
    }
}
