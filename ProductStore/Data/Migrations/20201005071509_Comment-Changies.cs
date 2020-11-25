using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductStore.Migrations
{
    public partial class CommentChangies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalMark",
                table: "Rating");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Order",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Mark",
                columns: table => new
                {
                    MarkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    TotalMark = table.Column<double>(nullable: false),
                    RatingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mark", x => x.MarkId);
                    table.ForeignKey(
                        name: "FK_Mark_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mark_Rating_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Rating",
                        principalColumn: "RatingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mark_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mark_ProductId",
                table: "Mark",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Mark_RatingId",
                table: "Mark",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Mark_UserId",
                table: "Mark",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mark");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Order");

            migrationBuilder.AddColumn<double>(
                name: "TotalMark",
                table: "Rating",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
