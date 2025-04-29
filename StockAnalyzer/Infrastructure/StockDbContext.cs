using Microsoft.EntityFrameworkCore;
using StockAnalyzer.Domain.Entities;

namespace StockAnalyzer.Infrastructure;

public class StockDbContext : DbContext
{
    public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }

    public DbSet<StockPrice> StockPrices { get; set; }
    public DbSet<WinRate> WinRates { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<PriceChange> PriceChanges { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
