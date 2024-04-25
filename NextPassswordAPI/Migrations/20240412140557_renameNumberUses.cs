using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextPassswordAPI.Migrations
{
    /// <inheritdoc />
    public partial class renameNumberUses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numberUses",
                table: "Tokens",
                newName: "NumberUses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberUses",
                table: "Tokens",
                newName: "numberUses");
        }
    }
}
