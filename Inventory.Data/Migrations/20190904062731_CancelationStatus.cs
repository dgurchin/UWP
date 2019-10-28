using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class CancelationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 5,
                column: "StatusIdBeginning",
                value: 2);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 9,
                column: "StatusIdBeginning",
                value: 3);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 12,
                column: "StatusIdBeginning",
                value: 4);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 15,
                column: "StatusIdBeginning",
                value: 5);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 17,
                column: "StatusIdBeginning",
                value: 6);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 19,
                column: "StatusIdBeginning",
                value: 7);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 21,
                column: "StatusIdBeginning",
                value: 8);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 23,
                column: "StatusIdBeginning",
                value: 9);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 25,
                column: "StatusIdBeginning",
                value: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 5,
                column: "StatusIdBeginning",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 9,
                column: "StatusIdBeginning",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 12,
                column: "StatusIdBeginning",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 15,
                column: "StatusIdBeginning",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 17,
                column: "StatusIdBeginning",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 19,
                column: "StatusIdBeginning",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 21,
                column: "StatusIdBeginning",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 23,
                column: "StatusIdBeginning",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 25,
                column: "StatusIdBeginning",
                value: 1);
        }
    }
}
