namespace StockAnalyzer.Domain.Dtos;

public class StockHoldDto
{
    public long Quantity { get; set; }
    public decimal BuyPrice { get; set; }
    public DateOnly BuyDate { get; set; }

    public decimal GetEarningInPercent(decimal currentPrice)
    {
        return (currentPrice - BuyPrice) / BuyPrice * 100;
    }

    public decimal GetAssetValue(decimal currentPrice)
    {
        return currentPrice * Quantity;
    }
}
