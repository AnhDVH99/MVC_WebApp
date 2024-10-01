using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_Core_MVC_Piacom.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceDetails_Products_ProductID",
                table: "PriceDetails");

            migrationBuilder.DropIndex(
                name: "IX_PriceDetails_ProductID",
                table: "PriceDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PriceDetails_ProductID",
                table: "PriceDetails",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceDetails_Products_ProductID",
                table: "PriceDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
