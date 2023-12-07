using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace discordbot.dal.Migrations
{
    /// <inheritdoc />
    public partial class datemissing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                schema: "as",
                table: "artLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date",
                schema: "as",
                table: "artLog");
        }
    }
}
