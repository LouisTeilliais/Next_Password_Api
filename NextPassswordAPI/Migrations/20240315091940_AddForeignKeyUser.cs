using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextPassswordAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Passwords_UserId",
                table: "Passwords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_AspNetUsers_UserId",
                table: "Passwords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_AspNetUsers_UserId",
                table: "Passwords");

            migrationBuilder.DropIndex(
                name: "IX_Passwords_UserId",
                table: "Passwords");
        }
    }
}
