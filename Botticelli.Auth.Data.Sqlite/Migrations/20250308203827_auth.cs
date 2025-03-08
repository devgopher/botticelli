using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Botticelli.Auth.Data.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class auth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Botticelli.Auth.Sample.Telegram");

            migrationBuilder.CreateTable(
                name: "BotUserRoles",
                schema: "Botticelli.Auth.Sample.Telegram",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleName = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    IsSuperUser = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotUsers",
                schema: "Botticelli.Auth.Sample.Telegram",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    NickName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 254, nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUsers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_BotUsers_BotUserRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Botticelli.Auth.Sample.Telegram",
                        principalTable: "BotUserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessHistory",
                schema: "Botticelli.Auth.Sample.Telegram",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EntityUserId = table.Column<string>(type: "TEXT", nullable: true),
                    TimestampUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsSuccess = table.Column<bool>(type: "INTEGER", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessHistory_BotUsers_EntityUserId",
                        column: x => x.EntityUserId,
                        principalSchema: "Botticelli.Auth.Sample.Telegram",
                        principalTable: "BotUsers",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                schema: "Botticelli.Auth.Sample.Telegram",
                table: "BotUserRoles",
                columns: new[] { "Id", "Description", "IsSuperUser", "RoleName" },
                values: new object[,]
                {
                    { new Guid("94854345-5355-4343-3447-1122244f55a8"), "A default authorized user role", false, "User" },
                    { new Guid("9947e363-4255-408d-b277-33402b9f07a1"), "A default user for guest", false, "Guest" },
                    { new Guid("d9887829-61a7-4947-9eb6-7faa66363f08"), "Bot users administrator", true, "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessHistory_EntityUserId",
                schema: "Botticelli.Auth.Sample.Telegram",
                table: "AccessHistory",
                column: "EntityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BotUserRoles_RoleName",
                schema: "Botticelli.Auth.Sample.Telegram",
                table: "BotUserRoles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BotUsers_RoleId",
                schema: "Botticelli.Auth.Sample.Telegram",
                table: "BotUsers",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessHistory",
                schema: "Botticelli.Auth.Sample.Telegram");

            migrationBuilder.DropTable(
                name: "BotUsers",
                schema: "Botticelli.Auth.Sample.Telegram");

            migrationBuilder.DropTable(
                name: "BotUserRoles",
                schema: "Botticelli.Auth.Sample.Telegram");
        }
    }
}
