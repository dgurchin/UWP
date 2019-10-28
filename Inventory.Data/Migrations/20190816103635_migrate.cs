using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class migrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RowGuid",
                table: "Customer",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerGuid",
                table: "Communication",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RowGuid",
                table: "Communication",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerGuid",
                table: "Address",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RowGuid",
                table: "Address",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "DbVersion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "3.2.3");

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
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CustomerGuid",
                table: "Communication");

            migrationBuilder.DropColumn(
                name: "RowGuid",
                table: "Communication");

            migrationBuilder.DropColumn(
                name: "CustomerGuid",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "RowGuid",
                table: "Address");

            migrationBuilder.UpdateData(
                table: "DbVersion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "3.2.2");

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
