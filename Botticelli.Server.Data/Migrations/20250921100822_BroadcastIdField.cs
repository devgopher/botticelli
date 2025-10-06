using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Botticelli.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class BroadcastIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "155a1281-61f2-4cea-b7b4-6f97b30d48d7");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "90283ad1-e01d-436b-879d-75bdab45fdfd");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "e56de249-c6df-4699-9874-96d8247e7e57");

            migrationBuilder.AddColumn<string>(
                name: "BroadcastId",
                table: "BroadcastAttachments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "ApplicationRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "693cd866-21b0-4003-84a6-cdf57c85a6a7", "09/21/2025 10:08:21", "viewer", "VIEWER" },
                    { "9422072d-2306-434d-86a8-300d1a55f2ff", "09/21/2025 10:08:21", "bot_manager", "BOT_MANAGER" },
                    { "abb60b3b-0b22-487f-a88f-9cac88d9e925", "09/21/2025 10:08:21", "admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BroadcastAttachments_BroadcastId",
                table: "BroadcastAttachments",
                column: "BroadcastId");

            migrationBuilder.AddForeignKey(
                name: "FK_BroadcastAttachments_Broadcasts_BroadcastId",
                table: "BroadcastAttachments",
                column: "BroadcastId",
                principalTable: "Broadcasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BroadcastAttachments_Broadcasts_BroadcastId",
                table: "BroadcastAttachments");

            migrationBuilder.DropIndex(
                name: "IX_BroadcastAttachments_BroadcastId",
                table: "BroadcastAttachments");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "693cd866-21b0-4003-84a6-cdf57c85a6a7");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "9422072d-2306-434d-86a8-300d1a55f2ff");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "abb60b3b-0b22-487f-a88f-9cac88d9e925");

            migrationBuilder.DropColumn(
                name: "BroadcastId",
                table: "BroadcastAttachments");

            migrationBuilder.InsertData(
                table: "ApplicationRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "155a1281-61f2-4cea-b7b4-6f97b30d48d7", "06/12/2025 11:09:25", "admin", "ADMIN" },
                    { "90283ad1-e01d-436b-879d-75bdab45fdfd", "06/12/2025 11:09:25", "bot_manager", "BOT_MANAGER" },
                    { "e56de249-c6df-4699-9874-96d8247e7e57", "06/12/2025 11:09:25", "viewer", "VIEWER" }
                });
        }
    }
}
