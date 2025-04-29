namespace StockAnalyzer.Domain.Entities
{
    public class WinRate : BaseEntity
    {
        public required string Symbol { get; set; }
        public decimal BuyAtRsi { get; set; }
        public decimal? ContinueBuyAtRsi1 { get; set; }
        public decimal? ContinueBuyAtRsi2 { get; set; }

        public decimal SellAtRsi { get; set; }
        public decimal CutLossRateInPercent { get; set; }
        public decimal? ContinueSellAtRsi1 { get; set; }
        public decimal? ContinueSellAtRsi2 { get; set; }

        public decimal BalanceZero { get; set; }
        
        public decimal BalanceAfter1Year { get; set; }
        public decimal BalanceAfter2Year { get; set; }
        public decimal BalanceAfter3Year { get; set; }
        public decimal BalanceAfter4Year { get; set; }
        public decimal BalanceAfter5Year { get; set; }
        public decimal BalanceAfter6Year { get; set; }
        public decimal BalanceAfter7Year { get; set; }
        public decimal BalanceAfter8Year { get; set; }
        public decimal BalanceAfter9Year { get; set; }
        public decimal BalanceAfter10Year { get; set; }

        // WinRate = Win Count / (Win Count + Loss Count)
        public decimal WinRateAfter1Year { get; set; }
        public decimal WinRateAfter2Year { get; set; }
        public decimal WinRateAfter3Year { get; set; }
        public decimal WinRateAfter4Year { get; set; }
        public decimal WinRateAfter5Year { get; set; }
        public decimal WinRateAfter6Year { get; set; }
        public decimal WinRateAfter7Year { get; set; }
        public decimal WinRateAfter8Year { get; set; }
        public decimal WinRateAfter9Year { get; set; }
        public decimal WinRateAfter10Year { get; set; }
    }
}
