using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemOrdering.Infrastructure.Migrations
{
    public partial class UpdateTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OrderedProduct",
                newName: "OrderedProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAndAmount_ShoppingCarts_ShoppingCartId",
                table: "ProductsAndAmount");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shop_ShopId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shop",
                table: "Shop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAndAmount",
                table: "ProductsAndAmount");

            migrationBuilder.RenameTable(
                name: "Shop",
                newName: "Shops");

            migrationBuilder.RenameTable(
                name: "ProductsAndAmount",
                newName: "ProductsAndAmount");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAndAmount_ShoppingCartId",
                table: "ProductsAndAmount",
                newName: "IX_ProductsAndAmount_ShoppingCartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shops",
                table: "Shops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsAndAmount",
                table: "ProductsAndAmount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shops_ShopId",
                table: "Products",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsAndAmount_ShoppingCarts_ShoppingCartId",
                table: "ProductsAndAmount",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shops_ShopId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsAndAmount_ShoppingCarts_ShoppingCartId",
                table: "ProductsAndAmount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shops",
                table: "Shops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsAndAmount",
                table: "ProductsAndAmount");

            migrationBuilder.RenameTable(
                name: "Shops",
                newName: "Shop");

            migrationBuilder.RenameTable(
                name: "ProductsAndAmount",
                newName: "ProductsAndAmount");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsAndAmount_ShoppingCartId",
                table: "ProductsAndAmount",
                newName: "IX_ProductAndAmount_ShoppingCartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shop",
                table: "Shop",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAndAmount",
                table: "ProductsAndAmount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAndAmount_ShoppingCarts_ShoppingCartId",
                table: "ProductsAndAmount",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shop_ShopId",
                table: "Products",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
