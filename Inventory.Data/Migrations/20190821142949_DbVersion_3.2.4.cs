using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class DbVersion_324 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DbVersion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "3.2.4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DbVersion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "3.2.3");
        }
    }
}
