using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopHoNo.Migrations
{
    public partial class UpdateOrderAndCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Thêm khóa ngoại vào bảng CartItem
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CartItem",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_UserId",
                table: "CartItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_AspNetUsers_UserId",
                table: "CartItem",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa khóa ngoại và trường UserId trong bảng CartItem
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_AspNetUsers_UserId",
                table: "CartItem");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_UserId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartItem");

            // Xóa khóa ngoại và trường UserId trong bảng Orders
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");
        }
    }
}
