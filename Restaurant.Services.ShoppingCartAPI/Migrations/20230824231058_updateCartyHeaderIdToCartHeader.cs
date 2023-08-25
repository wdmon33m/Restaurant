using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Services.ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateCartyHeaderIdToCartHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartHeaders_CartyHeaderId",
                table: "CartDetails");

            migrationBuilder.RenameColumn(
                name: "CartyHeaderId",
                table: "CartDetails",
                newName: "CartHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_CartyHeaderId",
                table: "CartDetails",
                newName: "IX_CartDetails_CartHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartHeaders_CartHeaderId",
                table: "CartDetails",
                column: "CartHeaderId",
                principalTable: "CartHeaders",
                principalColumn: "CartHeaderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartHeaders_CartHeaderId",
                table: "CartDetails");

            migrationBuilder.RenameColumn(
                name: "CartHeaderId",
                table: "CartDetails",
                newName: "CartyHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_CartHeaderId",
                table: "CartDetails",
                newName: "IX_CartDetails_CartyHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartHeaders_CartyHeaderId",
                table: "CartDetails",
                column: "CartyHeaderId",
                principalTable: "CartHeaders",
                principalColumn: "CartHeaderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
