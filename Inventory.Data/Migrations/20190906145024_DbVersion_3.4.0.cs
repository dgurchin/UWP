using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class DbVersion_340 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DbVersion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "3.4.0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DbVersion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "3.3.1");
        }
    }
}
