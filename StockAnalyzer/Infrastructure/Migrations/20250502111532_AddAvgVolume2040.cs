using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockAnalyzer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAvgVolume2040 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AvgVolume20",
                table: "StockPrices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AvgVolume40",
                table: "StockPrices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgVolume20",
                table: "StockPrices");

            migrationBuilder.DropColumn(
                name: "AvgVolume40",
                table: "StockPrices");
        }
    }
}
