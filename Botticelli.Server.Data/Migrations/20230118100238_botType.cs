using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Botticelli.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class botType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "BotInfo",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "BotInfo");
        }
    }
}
