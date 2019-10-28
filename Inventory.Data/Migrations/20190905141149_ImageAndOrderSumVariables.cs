using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class ImageAndOrderSumVariables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Variable",
                columns: new[] { "Id", "Data", "Description", "Name" },
                values: new object[] { 4, "68", "Ширина превью картинки", "ThumbnailWidth" });

            migrationBuilder.InsertData(
                table: "Variable",
                columns: new[] { "Id", "Data", "Description", "Name" },
                values: new object[] { 5, "56", "Высота превью картинки", "ThumbnailHeight" });

            migrationBuilder.InsertData(
                table: "Variable",
                columns: new[] { "Id", "Data", "Description", "Name" },
                values: new object[] { 6, "100", "Минимальная сума заказа", "MinimumOrderSum" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Variable",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Variable",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Variable",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
