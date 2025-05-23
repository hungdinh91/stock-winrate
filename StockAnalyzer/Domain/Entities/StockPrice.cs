﻿using Microsoft.EntityFrameworkCore;

namespace StockAnalyzer.Domain.Entities
{
    [Index(nameof(Symbol), nameof(Date))]
    public class StockPrice : BaseEntity
    {
        public required string Symbol { get; set; }
        public DateOnly Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public long Volume { get; set; }
        public double Rsi14 { get; set; }
        public decimal ChangeInPercent { get; set; }
        public long AvgVolume20 { get; set; }
        public long AvgVolume40 { get; set; }
        public long AvgVolume60 { get; set; }
    }
}
