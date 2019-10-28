using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class Courier_CourierOrder_StopSheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courier",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    RowGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StopSheet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowGuid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DishGuid = table.Column<Guid>(nullable: false),
                    DishId = table.Column<int>(nullable: true),
                    RestaurantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopSheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StopSheet_Dish_DishId",
                        column: x => x.DishId,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StopSheet_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourierOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowGuid = table.Column<Guid>(nullable: false),
                    CourierGuid = table.Column<Guid>(nullable: false),
                    CourierId = table.Column<int>(nullable: true),
                    OrderGuid = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourierOrder_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourierOrder_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Dish",
                keyColumn: "Id",
                keyValue: 37,
                column: "Description",
                value: @"Пепси, Миринда, Севен Ап, Газированная вода
В пределах заведения, единоразово.");

            migrationBuilder.CreateIndex(
                name: "IX_CourierOrder_CourierId",
                table: "CourierOrder",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierOrder_OrderId",
                table: "CourierOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StopSheet_DishId",
                table: "StopSheet",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_StopSheet_RestaurantId",
                table: "StopSheet",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourierOrder");

            migrationBuilder.DropTable(
                name: "StopSheet");

            migrationBuilder.DropTable(
                name: "Courier");

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
