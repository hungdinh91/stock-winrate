using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockAnalyzer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSymbolToWinRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "WinRates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "WinRates");
        }
    }
}
