using Microsoft.EntityFrameworkCore;
using StockAnalyzer.Domain.Entities;
using StockAnalyzer.Infrastructure;
using StockAnalyzer.Shared.CQRS;

namespace StockAnalyzer.Application.Commands;

public class CalculateChangesCommand(string symbol) : IRequest<bool>
{
    public string Symbol { get; set; } = symbol;
}

public enum StockChange
{
    Neutral,
    Increase,
    Decrease
}

public class CalculateChangesCommandHandler : BaseHandler, IRequestHandler<CalculateChangesCommand, bool>
{
    public CalculateChangesCommandHandler(StockDbContext stockDbContext) : base(stockDbContext)
    {
    }

    public async Task<bool> Handle(CalculateChangesCommand request)
    {
        var oldPriceChanges = await _stockDbContext.PriceChanges.Where(x => x.Symbol == request.Symbol).ToListAsync();
        var stockPrices = await _stockDbContext.StockPrices.Where(x => x.Symbol == request.Symbol).ToListAsync();
        stockPrices = stockPrices.OrderBy(x => x.Date).ToList();

        if (stockPrices.Count < 2)
        {
            return false;
        }

        var priceChanges = new List<PriceChange>();
        var lastStatus = StockChange.Neutral;
        var beginPrice = stockPrices[0];

        for (var i = 1; i < stockPrices.Count; i++)
        {
            var todayPrice = stockPrices[i];
            var yesterdayPrice = stockPrices[i - 1];

            if (todayPrice.ClosePrice == yesterdayPrice.ClosePrice) // if neutral
            {
                if (lastStatus == StockChange.Neutral) continue;
            }
            else if (todayPrice.ClosePrice < yesterdayPrice.ClosePrice) // if decrease
            {
                if (lastStatus == StockChange.Decrease) continue;
            }
            else // if increase
            {
                if (lastStatus == StockChange.Increase) continue;
            }

            priceChanges.Add(CreateChange(beginPrice, yesterdayPrice));
            beginPrice = todayPrice;
            lastStatus = todayPrice.ChangeInPercent == 0 ? StockChange.Neutral : (todayPrice.ChangeInPercent < 0 ? StockChange.Decrease : StockChange.Increase);
        }

        using (var trans = await _stockDbContext.Database.BeginTransactionAsync())
        {
            if (oldPriceChanges.Any()) _stockDbContext.PriceChanges.RemoveRange(oldPriceChanges);

            if (priceChanges.Any()) await _stockDbContext.PriceChanges.AddRangeAsync(priceChanges);

            await _stockDbContext.SaveChangesAsync();

            await trans.CommitAsync();
        }

        return true;
    }

    private PriceChange CreateChange(StockPrice beginPrice, StockPrice endPrice)
    {
        var closedPriceBefore = beginPrice.ClosePrice / (beginPrice.ChangeInPercent / 100 + 1);
        var periodChangeRate = (endPrice.ClosePrice - closedPriceBefore) / closedPriceBefore * 100;

        return new PriceChange
        {
            Symbol = beginPrice.Symbol,
            FromDate = beginPrice.Date,
            ToDate = endPrice.Date,
            TotalDays = endPrice.Date.DayNumber - beginPrice.Date.DayNumber + 1,
            TotalChangePrice = endPrice.ClosePrice - closedPriceBefore,
            ChangeInPercent = periodChangeRate
        };
    }
}
