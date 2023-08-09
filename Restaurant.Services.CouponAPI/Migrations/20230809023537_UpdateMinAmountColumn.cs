using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMinAmountColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MinAmount",
                table: "Coupons",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponID", "CouponCode", "DiscountAmount", "MinAmount" },
                values: new object[] { 1, "10OFF", 10.0, 20.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponID",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "MinAmount",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
