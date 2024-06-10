using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discordbot.dal.Migrations
{
    /// <inheritdoc />
    public partial class create_roles_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Servers_DiscordServerId",
                schema: "ff",
                table: "Servers",
                column: "DiscordServerId");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "ff",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscordRoleId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ParentDiscordServerId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    permission_level = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Roles_Servers_ParentDiscordServerId",
                        column: x => x.ParentDiscordServerId,
                        principalSchema: "ff",
                        principalTable: "Servers",
                        principalColumn: "DiscordServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ParentDiscordServerId",
                schema: "ff",
                table: "Roles",
                column: "ParentDiscordServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles",
                schema: "ff");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Servers_DiscordServerId",
                schema: "ff",
                table: "Servers");
        }
    }
}
