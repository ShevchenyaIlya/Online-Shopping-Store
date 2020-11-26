using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductStore.Migrations
{
    public partial class FixMistace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryPicture",
                table: "Products");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProductPicture",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPicture",
                table: "Products");

            migrationBuilder.AddColumn<byte[]>(
                name: "CategoryPicture",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
