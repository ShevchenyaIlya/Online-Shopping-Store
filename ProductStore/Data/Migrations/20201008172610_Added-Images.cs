using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductStore.Migrations
{
    public partial class AddedImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mark_Rating_RatingId",
                table: "Mark");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductBasket_ProductBasketBasketId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductBasketBasketId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Mark_RatingId",
                table: "Mark");

            migrationBuilder.DropColumn(
                name: "ProductBasketBasketId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Mark");

            migrationBuilder.AddColumn<int>(
                name: "UserMarksMarkId",
                table: "Rating",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CategoryPicture",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductsProductId",
                table: "ProductBasket",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CountryPicture",
                table: "Country",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_UserMarksMarkId",
                table: "Rating",
                column: "UserMarksMarkId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBasket_ProductsProductId",
                table: "ProductBasket",
                column: "ProductsProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBasket_Products_ProductsProductId",
                table: "ProductBasket",
                column: "ProductsProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Mark_UserMarksMarkId",
                table: "Rating",
                column: "UserMarksMarkId",
                principalTable: "Mark",
                principalColumn: "MarkId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBasket_Products_ProductsProductId",
                table: "ProductBasket");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Mark_UserMarksMarkId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_UserMarksMarkId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_ProductBasket_ProductsProductId",
                table: "ProductBasket");

            migrationBuilder.DropColumn(
                name: "UserMarksMarkId",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "CategoryPicture",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductsProductId",
                table: "ProductBasket");

            migrationBuilder.DropColumn(
                name: "CountryPicture",
                table: "Country");

            migrationBuilder.AddColumn<long>(
                name: "ProductBasketBasketId",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Mark",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductBasketBasketId",
                table: "Products",
                column: "ProductBasketBasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Mark_RatingId",
                table: "Mark",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mark_Rating_RatingId",
                table: "Mark",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "RatingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductBasket_ProductBasketBasketId",
                table: "Products",
                column: "ProductBasketBasketId",
                principalTable: "ProductBasket",
                principalColumn: "BasketId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
