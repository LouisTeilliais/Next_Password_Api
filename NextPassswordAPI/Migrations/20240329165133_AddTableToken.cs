using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextPassswordAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTableToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordToken",
                table: "Passwords");

            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedTokenId",
                table: "Passwords",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenValue = table.Column<string>(type: "text", nullable: true),
                    AssociatedPasswordId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Passwords_AssociatedPasswordId",
                        column: x => x.AssociatedPasswordId,
                        principalTable: "Passwords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AssociatedPasswordId",
                table: "Tokens",
                column: "AssociatedPasswordId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropColumn(
                name: "AssociatedTokenId",
                table: "Passwords");

            migrationBuilder.AddColumn<string>(
                name: "PasswordToken",
                table: "Passwords",
                type: "text",
                nullable: true);
        }
    }
}
