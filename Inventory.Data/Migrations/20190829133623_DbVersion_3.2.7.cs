using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class DbVersion_327 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DbVersion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "3.2.7");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DbVersion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "3.2.6");
        }
    }
}
