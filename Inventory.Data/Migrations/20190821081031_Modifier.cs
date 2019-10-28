using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class Modifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modifier",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowGuid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsRequired = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifier", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Dish",
                keyColumn: "Id",
                keyValue: 37,
                column: "Description",
                value: "Пепси, Миринда, Севен Ап, Газированная вода в пределах заведения, единоразово.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modifier");

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
