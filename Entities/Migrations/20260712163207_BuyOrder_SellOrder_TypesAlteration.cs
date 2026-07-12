using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class BuyOrder_SellOrder_TypesAlteration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StockSymbol",
                table: "SellOrders",
                type: "nvarchar(12)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockName",
                table: "SellOrders",
                type: "nvarchar(60)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockSymbol",
                table: "BuyOrders",
                type: "nvarchar(12)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockName",
                table: "BuyOrders",
                type: "nvarchar(60)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_SellOrders_Price",
                table: "SellOrders",
                sql: "[Price] >= 1 AND [Price] <= 100000");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SellOrders_Quantity",
                table: "SellOrders",
                sql: "[Quantity] >= 1 AND [Quantity] <= 100000");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BuyOrders_Price",
                table: "BuyOrders",
                sql: "[Price] >= 1 AND [Price] <= 100000");

            migrationBuilder.AddCheckConstraint(
                name: "CK_BuyOrders_Quantity",
                table: "BuyOrders",
                sql: "[Quantity] >= 1 AND [Quantity] <= 100000");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SellOrders_Price",
                table: "SellOrders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SellOrders_Quantity",
                table: "SellOrders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BuyOrders_Price",
                table: "BuyOrders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_BuyOrders_Quantity",
                table: "BuyOrders");

            migrationBuilder.AlterColumn<string>(
                name: "StockSymbol",
                table: "SellOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockName",
                table: "SellOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockSymbol",
                table: "BuyOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockName",
                table: "BuyOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldNullable: true);
        }
    }
}
