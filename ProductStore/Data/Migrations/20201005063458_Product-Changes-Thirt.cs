using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductStore.Migrations
{
    public partial class ProductChangesThirt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Country_CreatedPlaceCountryId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedPlaceCountryId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Country_CreatedPlaceCountryId",
                table: "Products",
                column: "CreatedPlaceCountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Country_CreatedPlaceCountryId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedPlaceCountryId",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Country_CreatedPlaceCountryId",
                table: "Products",
                column: "CreatedPlaceCountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
