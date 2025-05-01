using Microsoft.EntityFrameworkCore;

namespace StockAnalyzer.Domain.Entities;

public enum TransactionType
{
    Buy,
    Sell
}

[Index(nameof(Date))]
public class Transaction : BaseEntity
{
    public required string Symbol { get; set; }
    public int WinRateId { get; set; }
    public DateOnly Date { get; set; }
    public TransactionType Type { get; set; }
    public long Volume { get; set; }
    public decimal Price { get; set; }
    public decimal TotalMarketValue { get; set; }
    public decimal TotalCash { get; set; }
    public string? Description { get; set; }
    public decimal HoldingOriginalCash { get; set; }
    public decimal HoldingWinLossInPercent { get; set; }
}
