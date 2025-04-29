using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockAnalyzer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWinRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WinRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyAtRsi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContinueBuyAtRsi1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ContinueBuyAtRsi2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SellAtRsi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContinueSellAtRsi1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ContinueSellAtRsi2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BalanceZero = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter1Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter2Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter3Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter4Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter5Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter6Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter7Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter8Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter9Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter10Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter1Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter2Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter3Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter4Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter5Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter6Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter7Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter8Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter9Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinRateAfter10Year = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WinRates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WinRates");
        }
    }
}
