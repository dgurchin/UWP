using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class OrderStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Gold", "Новый с сайта" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "BurlyWood", "Уточнение" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Purple", "Оформление" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "DarkSlateBlue", "Ожидание" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Khaki", "Принят" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "SteelBlue", "Таймер" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "MediumSeaGreen", "В работе" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "OliveDrab", "Собран" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Gray", "Едет" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "White", "Доставлен" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "White", "Закрыт" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Maroon", "Отклонен" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "White", "Корзина" });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 2 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 3 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 12 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 2, 3 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 12 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 3, 2 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 3, 2 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 8,
                column: "StatusIdEnd",
                value: 5);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 12 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 4, 5 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 4, 6 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 13,
                column: "StatusIdEnd",
                value: 6);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 5, 7 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 12 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 6, 7 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 12 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 18,
                column: "StatusIdBeginning",
                value: 7);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 12 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 20,
                column: "StatusIdBeginning",
                value: 8);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 12 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 12 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 10, 11 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 12 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 12, 13 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Green", "В работе" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Snow", "Доставлен" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Gray", "Едет" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Snow", "Закрыт" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "FloralWhite", "Корректировка" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "LightGray", "Корзина" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Gold", "Новый с сайта" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Coral", "Отклонен" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Violet", "Отложен" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "LightBlue", "Уточнение" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "GreenYellow", "Принят" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "CadetBlue", "Собран" });

            migrationBuilder.UpdateData(
                table: "OrderStatus",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ColorStatus", "Name" },
                values: new object[] { "Aqua", "Таймер" });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 7, 8 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 10, 5 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 9, 5 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { null, 7 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { null, 11 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 2, 8 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 2, 4 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 8,
                column: "StatusIdEnd",
                value: 8);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 3, 2 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 12, 8 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 12, 3 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 13,
                column: "StatusIdEnd",
                value: 9);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 1, 8 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 13, 8 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 13, 1 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 11, 13 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 18,
                column: "StatusIdBeginning",
                value: 11);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 10, 8 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 20,
                column: "StatusIdBeginning",
                value: 10);

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 9, 11 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 8, 6 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 7, 10 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 7, 9 });

            migrationBuilder.UpdateData(
                table: "OrderStatusSequence",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 11, 1 });

            migrationBuilder.InsertData(
                table: "OrderStatusSequence",
                columns: new[] { "Id", "Algorithm", "Comment", "Direction", "StatusIdBeginning", "StatusIdEnd" },
                values: new object[] { 27, null, null, null, 5, 10 });
        }
    }
}
