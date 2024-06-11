using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discordbot.dal.Migrations
{
    /// <inheritdoc />
    public partial class Custom_Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomColorRoles",
                schema: "ff",
                columns: table => new
                {
                    CustomRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscordRoleId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ParentDiscordServerId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    OwnerUserDiscordId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleColorR = table.Column<byte>(type: "tinyint", nullable: false),
                    RoleColorG = table.Column<byte>(type: "tinyint", nullable: false),
                    RoleColorB = table.Column<byte>(type: "tinyint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomColorRoles", x => x.CustomRoleId);
                    table.ForeignKey(
                        name: "FK_CustomColorRoles_Servers_ParentDiscordServerId",
                        column: x => x.ParentDiscordServerId,
                        principalSchema: "ff",
                        principalTable: "Servers",
                        principalColumn: "DiscordServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomColorRoles_ParentDiscordServerId",
                schema: "ff",
                table: "CustomColorRoles",
                column: "ParentDiscordServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomColorRoles",
                schema: "ff");
        }
    }
}
