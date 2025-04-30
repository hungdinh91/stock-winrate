namespace StockAnalyzer.Domain.Dtos;

public class CalculateWinRateDto
{
    public decimal BuyAtRsi { get; set; } = 30;
    public decimal? ContinueBuyAtRsi1 { get; set; }
    public decimal? ContinueBuyAtRsi2 { get; set; }

    public decimal SellAtRsi { get; set; } = 70;
    public decimal? ContinueSellAtRsi1 { get; set; }
    public decimal? ContinueSellAtRsi2 { get; set; }
    public decimal CutLossRateInPercent { get; set; } = 7;

    public decimal BalanceZero { get; set; } = 1000000000;
    public string InvestFrom { get; set; } = "2010-01-01";
    public decimal MaxDayTransAmount { get; set; }
    public decimal MonthlyInvestAmount { get; set; }
}
