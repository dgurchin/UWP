using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class DishMark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RowGuid",
                table: "DishMark",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Dish",
                keyColumn: "Id",
                keyValue: 37,
                column: "Description",
                value: @"Пепси, Миринда, Севен Ап, Газированная вода
В пределах заведения, единоразово.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowGuid",
                table: "DishMark");

            migrationBuilder.UpdateData(
                table: "Dish",
                keyColumn: "Id",
                keyValue: 37,
                column: "Description",
                value: @"Пепси, Миринда, Севен Ап, Газированная вода
В пределах заведения, единоразово.");
        }
    }
}
