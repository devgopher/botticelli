using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Botticelli.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class broadcasting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BroadcastAttachments_Broadcasts_BroadcastBotId",
                table: "BroadcastAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Broadcasts",
                table: "Broadcasts");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "0704f04d-1faa-44ab-b0fa-3ec05a168255");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "1027bf58-8ea4-4042-9651-690ef7eff777");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "19291d61-00bc-497c-8162-aa1ed022d1a7");

            migrationBuilder.RenameColumn(
                name: "BroadcastBotId",
                table: "BroadcastAttachments",
                newName: "BroadcastId");

            migrationBuilder.RenameIndex(
                name: "IX_BroadcastAttachments_BroadcastBotId",
                table: "BroadcastAttachments",
                newName: "IX_BroadcastAttachments_BroadcastId");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Broadcasts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Received",
                table: "Broadcasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Sent",
                table: "Broadcasts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Broadcasts",
                table: "Broadcasts",
                column: "Id");

            migrationBuilder.InsertData(
                table: "ApplicationRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e960a01-b27e-4435-961c-231e9276a6a4", "03/10/2025 19:37:25", "viewer", "VIEWER" },
                    { "85ca9cb0-e520-461b-8b2e-98877f519efd", "03/10/2025 19:37:25", "bot_manager", "BOT_MANAGER" },
                    { "ebdb9ace-7a62-41cf-b4e3-749be50310e2", "03/10/2025 19:37:25", "admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BroadcastAttachments_Broadcasts_BroadcastId",
                table: "BroadcastAttachments",
                column: "BroadcastId",
                principalTable: "Broadcasts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BroadcastAttachments_Broadcasts_BroadcastId",
                table: "BroadcastAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Broadcasts",
                table: "Broadcasts");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "2e960a01-b27e-4435-961c-231e9276a6a4");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "85ca9cb0-e520-461b-8b2e-98877f519efd");

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: "ebdb9ace-7a62-41cf-b4e3-749be50310e2");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Broadcasts");

            migrationBuilder.DropColumn(
                name: "Received",
                table: "Broadcasts");

            migrationBuilder.DropColumn(
                name: "Sent",
                table: "Broadcasts");

            migrationBuilder.RenameColumn(
                name: "BroadcastId",
                table: "BroadcastAttachments",
                newName: "BroadcastBotId");

            migrationBuilder.RenameIndex(
                name: "IX_BroadcastAttachments_BroadcastId",
                table: "BroadcastAttachments",
                newName: "IX_BroadcastAttachments_BroadcastBotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Broadcasts",
                table: "Broadcasts",
                column: "BotId");

            migrationBuilder.InsertData(
                table: "ApplicationRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0704f04d-1faa-44ab-b0fa-3ec05a168255", "12/01/2024 15:54:52", "viewer", "VIEWER" },
                    { "1027bf58-8ea4-4042-9651-690ef7eff777", "12/01/2024 15:54:52", "admin", "ADMIN" },
                    { "19291d61-00bc-497c-8162-aa1ed022d1a7", "12/01/2024 15:54:52", "bot_manager", "BOT_MANAGER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BroadcastAttachments_Broadcasts_BroadcastBotId",
                table: "BroadcastAttachments",
                column: "BroadcastBotId",
                principalTable: "Broadcasts",
                principalColumn: "BotId");
        }
    }
}
