using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class initilizeRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "IsActive", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5086bb87-f135-4627-b808-0ee89479f02e", null, "ApplicationRole", true, "ADMIN", null },
                    { "88823985-b17d-4828-b481-005cf482f7e0", null, "ApplicationRole", true, "CUSTOMER", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5086bb87-f135-4627-b808-0ee89479f02e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88823985-b17d-4828-b481-005cf482f7e0");
        }
    }
}
