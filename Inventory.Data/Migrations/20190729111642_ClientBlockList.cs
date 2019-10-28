using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class ClientBlockList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIgnore",
                table: "Communication");

            migrationBuilder.AddColumn<bool>(
                name: "IsBlockList",
                table: "Customer",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlockList",
                table: "Customer");

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnore",
                table: "Communication",
                nullable: false,
                defaultValue: false);
        }
    }
}
