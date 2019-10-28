using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class MenuFolderParentGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuFolder_MenuFolder_ParentId",
                table: "MenuFolder");

            migrationBuilder.DropIndex(
                name: "IX_MenuFolder_ParentId",
                table: "MenuFolder");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MenuFolder");

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "Modifier",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentGuid",
                table: "MenuFolder",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuFolder_ParentGuid",
                table: "MenuFolder",
                column: "ParentGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuFolder_MenuFolder_ParentGuid",
                table: "MenuFolder",
                column: "ParentGuid",
                principalTable: "MenuFolder",
                principalColumn: "RowGuid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuFolder_MenuFolder_ParentGuid",
                table: "MenuFolder");

            migrationBuilder.DropIndex(
                name: "IX_MenuFolder_ParentGuid",
                table: "MenuFolder");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "Modifier");

            migrationBuilder.DropColumn(
                name: "ParentGuid",
                table: "MenuFolder");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "MenuFolder",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuFolder_ParentId",
                table: "MenuFolder",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuFolder_MenuFolder_ParentId",
                table: "MenuFolder",
                column: "ParentId",
                principalTable: "MenuFolder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
