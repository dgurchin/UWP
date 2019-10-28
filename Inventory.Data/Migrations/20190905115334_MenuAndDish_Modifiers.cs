using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class MenuAndDish_Modifiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Modifier_RowGuid",
                table: "Modifier",
                column: "RowGuid");

            migrationBuilder.CreateTable(
                name: "DishModifier",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowGuid = table.Column<Guid>(nullable: false),
                    DishGuid = table.Column<Guid>(nullable: false),
                    ModifierGuid = table.Column<Guid>(nullable: false),
                    IsRequired = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishModifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishModifier_Dish_DishGuid",
                        column: x => x.DishGuid,
                        principalTable: "Dish",
                        principalColumn: "RowGuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishModifier_Modifier_ModifierGuid",
                        column: x => x.ModifierGuid,
                        principalTable: "Modifier",
                        principalColumn: "RowGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuFolderModifier",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RowGuid = table.Column<Guid>(nullable: false),
                    MenuFolderGuid = table.Column<Guid>(nullable: false),
                    ModifierGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuFolderModifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuFolderModifier_MenuFolder_MenuFolderGuid",
                        column: x => x.MenuFolderGuid,
                        principalTable: "MenuFolder",
                        principalColumn: "RowGuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuFolderModifier_Modifier_ModifierGuid",
                        column: x => x.ModifierGuid,
                        principalTable: "Modifier",
                        principalColumn: "RowGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishModifier_DishGuid",
                table: "DishModifier",
                column: "DishGuid");

            migrationBuilder.CreateIndex(
                name: "IX_DishModifier_ModifierGuid",
                table: "DishModifier",
                column: "ModifierGuid");

            migrationBuilder.CreateIndex(
                name: "IX_MenuFolderModifier_MenuFolderGuid",
                table: "MenuFolderModifier",
                column: "MenuFolderGuid");

            migrationBuilder.CreateIndex(
                name: "IX_MenuFolderModifier_ModifierGuid",
                table: "MenuFolderModifier",
                column: "ModifierGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishModifier");

            migrationBuilder.DropTable(
                name: "MenuFolderModifier");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Modifier_RowGuid",
                table: "Modifier");
        }
    }
}
