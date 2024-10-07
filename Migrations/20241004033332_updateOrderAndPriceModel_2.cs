using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_Core_MVC_Piacom.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderAndPriceModel_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PriceDetails_ProductID",
                table: "PriceDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PriceDetails_UnitID",
                table: "PriceDetails",
                column: "UnitID");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceDetails_Products_ProductID",
                table: "PriceDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceDetails_Units_UnitID",
                table: "PriceDetails",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "UnitID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceDetails_Products_ProductID",
                table: "PriceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceDetails_Units_UnitID",
                table: "PriceDetails");

            migrationBuilder.DropIndex(
                name: "IX_PriceDetails_ProductID",
                table: "PriceDetails");

            migrationBuilder.DropIndex(
                name: "IX_PriceDetails_UnitID",
                table: "PriceDetails");
        }
    }
}
