using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Botticelli.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotInfo",
                columns: table => new
                {
                    BotId = table.Column<string>(type: "TEXT", nullable: false),
                    BotName = table.Column<string>(type: "TEXT", nullable: false),
                    LastKeepAlive = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: true),
                    BotKey = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotInfo", x => x.BotId);
                });

            migrationBuilder.CreateTable(
                name: "BroadcastAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MediaType = table.Column<int>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BroadcastAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Broadcasts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    BotId = table.Column<string>(type: "TEXT", nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Received = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Broadcasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotAdditionalInfo",
                columns: table => new
                {
                    BotId = table.Column<string>(type: "TEXT", nullable: false),
                    ItemName = table.Column<string>(type: "TEXT", nullable: false),
                    ItemValue = table.Column<string>(type: "TEXT", nullable: true),
                    BotInfoBotId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotAdditionalInfo", x => x.BotId);
                    table.ForeignKey(
                        name: "FK_BotAdditionalInfo_BotInfo_BotInfoBotId",
                        column: x => x.BotInfoBotId,
                        principalTable: "BotInfo",
                        principalColumn: "BotId");
                });

            migrationBuilder.InsertData(
                table: "ApplicationRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "155a1281-61f2-4cea-b7b4-6f97b30d48d7", "06/12/2025 11:09:25", "admin", "ADMIN" },
                    { "90283ad1-e01d-436b-879d-75bdab45fdfd", "06/12/2025 11:09:25", "bot_manager", "BOT_MANAGER" },
                    { "e56de249-c6df-4699-9874-96d8247e7e57", "06/12/2025 11:09:25", "viewer", "VIEWER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotAdditionalInfo_BotInfoBotId",
                table: "BotAdditionalInfo",
                column: "BotInfoBotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationRoles");

            migrationBuilder.DropTable(
                name: "ApplicationUserRoles");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "BotAdditionalInfo");

            migrationBuilder.DropTable(
                name: "BroadcastAttachments");

            migrationBuilder.DropTable(
                name: "Broadcasts");

            migrationBuilder.DropTable(
                name: "BotInfo");
        }
    }
}
