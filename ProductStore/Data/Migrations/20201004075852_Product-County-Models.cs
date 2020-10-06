using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductStore.Migrations
{
    public partial class ProductCountyModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(nullable: true),
                    CountryAbbreviation = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(nullable: true),
                    ProductDescription = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    CreatedPlaceCountryId = table.Column<int>(nullable: true),
                    PriceForOneKilogram = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Weight = table.Column<float>(nullable: false),
                    Protein = table.Column<float>(nullable: false),
                    Fat = table.Column<float>(nullable: false),
                    Carbohydrates = table.Column<float>(nullable: false),
                    EnergyValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Country_CreatedPlaceCountryId",
                        column: x => x.CreatedPlaceCountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedPlaceCountryId",
                table: "Products",
                column: "CreatedPlaceCountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
