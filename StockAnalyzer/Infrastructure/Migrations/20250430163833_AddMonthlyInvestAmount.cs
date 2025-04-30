using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockAnalyzer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthlyInvestAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyInvestAmount",
                table: "WinRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyInvestAmount",
                table: "WinRates");
        }
    }
}
