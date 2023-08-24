using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateCouponsObjectWithProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Coupons",
                table: "Coupons");

            migrationBuilder.RenameTable(
                name: "Coupons",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_Coupons_Name",
                table: "Products",
                newName: "IX_Products_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Coupons");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Name",
                table: "Coupons",
                newName: "IX_Coupons_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coupons",
                table: "Coupons",
                column: "ProductId");
        }
    }
}
