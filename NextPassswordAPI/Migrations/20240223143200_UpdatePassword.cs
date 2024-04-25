using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextPassswordAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Items",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "Items",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Items",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "Items",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Items",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Items",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Items",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Items",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Items",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Items",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Items",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Items",
                newName: "password");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
