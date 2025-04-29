namespace StockAnalyzer.Domain.Entities;

public class PriceChange : BaseEntity
{
    public required string Symbol { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public int TotalDays { get; set; }
    public decimal TotalChangePrice { get; set; }
    public decimal ChangeInPercent { get; set; }
}
