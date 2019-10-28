using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class RegistrationStatusEnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 7,
                column: "StatusIdEnd",
                value: 4);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 7,
                column: "StatusIdEnd",
                value: 2);
        }
    }
}
