using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductStore.Migrations
{
    public partial class UpdateProductBasket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "ProductBasket",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBasket_OrderId",
                table: "ProductBasket",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBasket_Order_OrderId",
                table: "ProductBasket",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBasket_Order_OrderId",
                table: "ProductBasket");

            migrationBuilder.DropIndex(
                name: "IX_ProductBasket_OrderId",
                table: "ProductBasket");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductBasket");
        }
    }
}
