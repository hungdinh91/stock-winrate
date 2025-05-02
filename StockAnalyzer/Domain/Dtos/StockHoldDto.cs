namespace StockAnalyzer.Domain.Dtos;

public class StockHoldDto
{
    public long TotalQuantity { get { return SellableQuantity + KeptHoldingStocks.Sum(x => x.Quantity); } }
    public decimal AvgBuyPrice { get; set; }
    public DateOnly LastBuyDate { get; set; }
    public decimal TotalOriginalCash { get; set; }
    public long SellableQuantity { get; set; }
    public List<KeptHoldingStock> KeptHoldingStocks { get; set; } = new List<KeptHoldingStock>();

    public decimal GetEarningInPercent(decimal currentPrice)
    {
        return (currentPrice - AvgBuyPrice) / AvgBuyPrice * 100;
    }

    public decimal GetAssetValue(decimal currentPrice)
    {
        return currentPrice * TotalQuantity;
    }

    public void BuyMore(long quantity, DateOnly date, decimal cashUsed)
    {
        LastBuyDate = date;
        TotalOriginalCash += cashUsed;
        KeptHoldingStocks.Add(new KeptHoldingStock { Quantity = quantity, BuyDate = date });
        AvgBuyPrice = TotalOriginalCash / TotalQuantity;
    }

    public void AddjustSellableQty(DateOnly date)
    {
        var sellableHistories = KeptHoldingStocks.Where(x => x.BuyDate.AddDays(3) <= date).ToList();
        sellableHistories.ForEach(x => KeptHoldingStocks.Remove(x));
        SellableQuantity += sellableHistories.Sum(x => x.Quantity);
    }

    
}

public class KeptHoldingStock
{
    public long Quantity { get; set; }
    public DateOnly BuyDate { get; set; }
}
