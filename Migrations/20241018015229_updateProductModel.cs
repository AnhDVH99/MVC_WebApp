using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_Core_MVC_Piacom.Migrations
{
    /// <inheritdoc />
    public partial class updateProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Units_UnitID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceDetails_Prices_PriceID",
                table: "PriceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceDetails_Products_ProductID",
                table: "PriceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceDetails_Units_UnitID",
                table: "PriceDetails");

            migrationBuilder.AlterColumn<string>(
                name: "SysU",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Units_UnitID",
                table: "OrderDetails",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "UnitID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceDetails_Prices_PriceID",
                table: "PriceDetails",
                column: "PriceID",
                principalTable: "Prices",
                principalColumn: "PriceID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceDetails_Products_ProductID",
                table: "PriceDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceDetails_Units_UnitID",
                table: "PriceDetails",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "UnitID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Units_UnitID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceDetails_Prices_PriceID",
                table: "PriceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceDetails_Products_ProductID",
                table: "PriceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceDetails_Units_UnitID",
                table: "PriceDetails");

            migrationBuilder.AlterColumn<string>(
                name: "SysU",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Units_UnitID",
                table: "OrderDetails",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "UnitID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceDetails_Prices_PriceID",
                table: "PriceDetails",
                column: "PriceID",
                principalTable: "Prices",
                principalColumn: "PriceID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceDetails_Products_ProductID",
                table: "PriceDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceDetails_Units_UnitID",
                table: "PriceDetails",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "UnitID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
