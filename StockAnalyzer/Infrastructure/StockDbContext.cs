using Microsoft.EntityFrameworkCore;
using StockAnalyzer.Domain.Entities;

namespace StockAnalyzer.Infrastructure;

public class StockDbContext : DbContext
{
    public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }

    public DbSet<StockPrice> StockPrices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
