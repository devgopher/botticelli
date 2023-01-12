using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Botticelli.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotInfo",
                columns: table => new
                {
                    BotId = table.Column<string>(type: "TEXT", nullable: false),
                    BotName = table.Column<string>(type: "TEXT", nullable: true),
                    LastKeepAlive = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotInfo", x => x.BotId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotInfo");
        }
    }
}
