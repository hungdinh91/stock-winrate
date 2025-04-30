using Microsoft.EntityFrameworkCore;
using StockAnalyzer.Domain.Dtos;
using StockAnalyzer.Domain.Entities;
using StockAnalyzer.Infrastructure;
using StockAnalyzer.Shared.CQRS;

namespace StockAnalyzer.Application.Commands;

public class CalculateWinRateCommand : CalculateWinRateDto, IRequest<WinRate>
{
    public string Symbol { get; set; }
    public new DateOnly InvestFrom { get; set; }
    public CalculateWinRateCommand(string symbol, CalculateWinRateDto dto)
    {
        Symbol = symbol;
        BalanceZero = dto.BalanceZero;
        BuyAtRsi = dto.BuyAtRsi;
        ContinueBuyAtRsi1 = dto.ContinueBuyAtRsi1;
        ContinueBuyAtRsi2 = dto.ContinueBuyAtRsi2;
        SellAtRsi = dto.SellAtRsi;
        ContinueSellAtRsi1 = dto.ContinueSellAtRsi1;
        ContinueSellAtRsi2 = dto.ContinueSellAtRsi2;
        CutLossRateInPercent = dto.CutLossRateInPercent;

        if (!DateOnly.TryParse(dto.InvestFrom, out DateOnly date)) throw new ArgumentException("InvestFrom is not valid");
        InvestFrom = date;
    }
}

public class CalculateWinRateCommandHandler : IRequestHandler<CalculateWinRateCommand, WinRate>
{
    private readonly StockDbContext _stockDbContext;

    public CalculateWinRateCommandHandler(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }

    public async Task<WinRate> Handle(CalculateWinRateCommand request)
    {
        // clear old winrates and transaction
        var obsoltetedWinrate = await _stockDbContext.WinRates
            .Where(x => x.Symbol == request.Symbol)
            .Where(x => x.BalanceZero == request.BalanceZero)
            .Where(x => x.BuyAtRsi == request.BuyAtRsi)
            .Where(x => x.ContinueBuyAtRsi1 == request.ContinueBuyAtRsi1)
            .Where(x => x.ContinueBuyAtRsi2 == request.ContinueBuyAtRsi2)
            .Where(x => x.SellAtRsi == request.SellAtRsi)
            .Where(x => x.ContinueSellAtRsi1 == request.ContinueSellAtRsi1)
            .Where(x => x.ContinueSellAtRsi2 == request.ContinueSellAtRsi2)
            .Where(x => x.CutLossRateInPercent == request.CutLossRateInPercent)
            .Where(x => x.InvestFrom == request.InvestFrom)
            .FirstOrDefaultAsync();

        var obsoltetedTrans = new List<Transaction>();
        if (obsoltetedWinrate != null)
        {
            obsoltetedTrans = await _stockDbContext.Transactions.Where(x => x.WinRateId == obsoltetedWinrate.Id).ToListAsync();
        }

        var stockPrices = await _stockDbContext.StockPrices.Where(x => x.Symbol == request.Symbol).ToListAsync();
        stockPrices = stockPrices.OrderBy(x => x.Date).ToList();

        var balance = request.BalanceZero;
        var trans = new List<Transaction>();
        var holdingStock = new StockHoldDto();
        var winRate = new WinRate()
        {
            Symbol = request.Symbol,
            SellAtRsi = request.SellAtRsi,
            ContinueSellAtRsi1 = request.ContinueSellAtRsi1,
            ContinueSellAtRsi2 = request.ContinueSellAtRsi2,
            BuyAtRsi = request.BuyAtRsi,
            ContinueBuyAtRsi1 = request.ContinueBuyAtRsi1,
            ContinueBuyAtRsi2 = request.ContinueBuyAtRsi2,
            BalanceZero = request.BalanceZero,
            CutLossRateInPercent = request.CutLossRateInPercent,
            InvestFrom = request.InvestFrom,
        };

        for (var i = 0; i < stockPrices.Count - 1; i++)
        {
            if (i + 1 < 14) continue;
            var thisDayStockPrice = stockPrices[i];
            var theNextDayStockPrice = stockPrices[i + 1];
            var winCount = 0;
            var lossCount = 0;
            if (thisDayStockPrice.Date < request.InvestFrom) continue;

            // Sell
            if (holdingStock.Quantity > 0 && (theNextDayStockPrice.Date.DayNumber - holdingStock.BuyDate.DayNumber) > 2)
            {
                var erarningInPercent = holdingStock.GetEarningInPercent(thisDayStockPrice.ClosePrice);

                // Check if need to cut loss first
                if (erarningInPercent < -request.CutLossRateInPercent || ShouldSell(thisDayStockPrice, request.SellAtRsi))
                {
                    // sell with next day open price
                    balance += holdingStock.Quantity * theNextDayStockPrice.OpenPrice;
                    trans.Add(new Transaction
                    {
                        Symbol = request.Symbol,
                        Date = theNextDayStockPrice.Date,
                        Price = theNextDayStockPrice.OpenPrice,
                        Volume = holdingStock.Quantity,
                        TotalCash = balance + holdingStock.Quantity * theNextDayStockPrice.OpenPrice,
                        Type = TransactionType.Sell,
                        Description = erarningInPercent < -request.CutLossRateInPercent ? "Cut loss " : "RSI low"
                    });
                    holdingStock.Quantity = 0;
                }

                if (erarningInPercent > 0) winCount++; else if (erarningInPercent < 0) lossCount++;
            }

            // Buy
            if (balance > theNextDayStockPrice.OpenPrice * 100 && ShouldBuy(stockPrices[i], request.BuyAtRsi))
            {
                // Buy in next day open price
                var quantity = balance / theNextDayStockPrice.OpenPrice;
                holdingStock.Quantity = (long)quantity;
                holdingStock.BuyPrice = theNextDayStockPrice.OpenPrice;
                holdingStock.BuyDate = theNextDayStockPrice.Date;
                balance -= holdingStock.Quantity * theNextDayStockPrice.OpenPrice;
                trans.Add(new Transaction
                {
                    Symbol = request.Symbol,
                    Date = theNextDayStockPrice.Date,
                    Price = theNextDayStockPrice.OpenPrice,
                    Volume = holdingStock.Quantity,
                    TotalMarketValue = holdingStock.Quantity * theNextDayStockPrice.OpenPrice,
                    TotalCash = balance,
                    Type = TransactionType.Buy,
                });
            }

            // Update winrate and balance after each period
            var totalBalance = balance + holdingStock.Quantity * theNextDayStockPrice.OpenPrice;
            var winRateInPercent = winCount + lossCount > 0 ? (decimal)winCount / (winCount + lossCount) : 0;
            var dayCount = (thisDayStockPrice.Date.DayNumber - request.InvestFrom.DayNumber);
            if (dayCount >= 52 * 5 * 1 && winRate.BalanceAfter1Year == 0) { winRate.BalanceAfter1Year = totalBalance; winRate.WinRateAfter1Year = winRateInPercent; }
            if (dayCount >= 52 * 5 * 2 && winRate.BalanceAfter2Year == 0) { winRate.BalanceAfter2Year = totalBalance; winRate.WinRateAfter2Year = winRateInPercent; }
            if (dayCount >= 52 * 5 * 3 && winRate.BalanceAfter3Year == 0) { winRate.BalanceAfter3Year = totalBalance; winRate.WinRateAfter3Year = winRateInPercent; }
            if (dayCount >= 52 * 5 * 4 && winRate.BalanceAfter4Year == 0) { winRate.BalanceAfter4Year = totalBalance; winRate.WinRateAfter4Year = winRateInPercent; }
            if (dayCount >= 52 * 5 * 5 && winRate.BalanceAfter5Year == 0) { winRate.BalanceAfter5Year = totalBalance; winRate.WinRateAfter5Year = winRateInPercent; }
            if (dayCount >= 52 * 5 * 6 && winRate.BalanceAfter6Year == 0) { winRate.BalanceAfter6Year = totalBalance; winRate.WinRateAfter6Year = winRateInPercent; }
            if (dayCount >= 52 * 5 * 7 && winRate.BalanceAfter7Year == 0) { winRate.BalanceAfter7Year = totalBalance; winRate.WinRateAfter7Year = winRateInPercent; }
            if (dayCount >= 52 * 5 * 8 && winRate.BalanceAfter8Year == 0) { winRate.BalanceAfter8Year = totalBalance; winRate.WinRateAfter8Year = winRateInPercent; }
            if (dayCount >= 52 * 5 * 9 && winRate.BalanceAfter9Year == 0) { winRate.BalanceAfter9Year = totalBalance; winRate.WinRateAfter9Year = winRateInPercent; }
            if (dayCount >= 52 * 5 * 10 && winRate.BalanceAfter10Year == 0) { winRate.BalanceAfter10Year = totalBalance; winRate.WinRateAfter10Year = winRateInPercent; }
        }

        using (var dbTrans = await _stockDbContext.Database.BeginTransactionAsync())
        {
            if (obsoltetedWinrate != null)
            {
                _stockDbContext.WinRates.Remove(obsoltetedWinrate);
            }

            if (obsoltetedTrans.Any())
            {
                _stockDbContext.Transactions.RemoveRange(obsoltetedTrans);
            }

            await _stockDbContext.WinRates.AddAsync(winRate);
            await _stockDbContext.SaveChangesAsync();

            if (trans.Any())
            {
                trans.ForEach(x => x.WinRateId = winRate.Id);
                await _stockDbContext.Transactions.AddRangeAsync(trans);
            }

            await _stockDbContext.SaveChangesAsync();
            await dbTrans.CommitAsync();
        }

        return winRate;
    }

    private bool ShouldBuy(StockPrice stockPrice, decimal rsiBuy)
    {
        return stockPrice.Rsi14 < (double)rsiBuy;
    }

    private bool ShouldSell(StockPrice stockPrice, decimal rsiSell)
    {
        return stockPrice.Rsi14 > (double)rsiSell;
    }
}
