using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockAnalyzer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxDayAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxDayTransAmount",
                table: "WinRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxDayTransAmount",
                table: "WinRates");
        }
    }
}
