using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockAnalyzer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfter11Year",
                table: "WinRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfter12Year",
                table: "WinRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfter13Year",
                table: "WinRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfter14Year",
                table: "WinRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfter15Year",
                table: "WinRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceFinal",
                table: "WinRates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HoldingOriginalCash",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HoldingWinLossInPercent",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAfter11Year",
                table: "WinRates");

            migrationBuilder.DropColumn(
                name: "BalanceAfter12Year",
                table: "WinRates");

            migrationBuilder.DropColumn(
                name: "BalanceAfter13Year",
                table: "WinRates");

            migrationBuilder.DropColumn(
                name: "BalanceAfter14Year",
                table: "WinRates");

            migrationBuilder.DropColumn(
                name: "BalanceAfter15Year",
                table: "WinRates");

            migrationBuilder.DropColumn(
                name: "BalanceFinal",
                table: "WinRates");

            migrationBuilder.DropColumn(
                name: "HoldingOriginalCash",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "HoldingWinLossInPercent",
                table: "Transactions");
        }
    }
}
