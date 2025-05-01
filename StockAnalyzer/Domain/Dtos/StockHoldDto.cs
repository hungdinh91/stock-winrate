namespace StockAnalyzer.Domain.Dtos;

public class StockHoldDto
{
    public long Quantity { get; set; }
    public decimal AvgBuyPrice { get; set; }
    public DateOnly BuyDate { get; set; }
    public decimal TotalOriginalCash { get { return Quantity * AvgBuyPrice; } }

    public decimal GetEarningInPercent(decimal currentPrice)
    {
        return (currentPrice - AvgBuyPrice) / AvgBuyPrice * 100;
    }

    public decimal GetAssetValue(decimal currentPrice)
    {
        return currentPrice * Quantity;
    }
}
