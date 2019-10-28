using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class PaymentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Касса");

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Кредитная карта");

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Оплата на сайте");

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Безнал");

            migrationBuilder.InsertData(
                table: "PaymentType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 5, "Сертификат" },
                    { 6, "Взаимозачет" },
                    { 7, "Неплательщики" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Наличные");

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Интернет");

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Банк. карточка");

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Перечисление");
        }
    }
}
