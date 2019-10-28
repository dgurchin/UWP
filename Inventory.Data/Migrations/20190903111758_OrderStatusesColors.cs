using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class OrderStatusesColors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "ColorStatus",
                value: "Plum");

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "ColorStatus",
                value: "MediumPurple");

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 8,
                column: "ColorStatus",
                value: "YellowGreen");

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 12,
                column: "ColorStatus",
                value: "IndianRed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "ColorStatus",
                value: "Purple");

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "ColorStatus",
                value: "DarkSlateBlue");

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 8,
                column: "ColorStatus",
                value: "OliveDrab");

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 12,
                column: "ColorStatus",
                value: "Maroon");
        }
    }
}
