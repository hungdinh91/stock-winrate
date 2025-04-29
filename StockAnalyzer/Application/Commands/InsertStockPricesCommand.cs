using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using StockAnalyzer.Domain.Entities;
using StockAnalyzer.Infrastructure;
using StockAnalyzer.Shared.CQRS;

namespace StockAnalyzer.Application.Commands;

public class InsertStockPricesCommand : IRequest<bool>
{
    public List<string[]>? StockPrices { get; set; }

    public InsertStockPricesCommand(List<string[]>? stockPrices)
    {
        StockPrices = stockPrices;
    }
}

public class InsertStockPricesCommandHandler : IRequestHandler<InsertStockPricesCommand, bool>
{
    private readonly StockDbContext _stockDbContext;

    public InsertStockPricesCommandHandler(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }

    public async Task<bool> Handle(InsertStockPricesCommand request)
    {
        if (request.StockPrices == null || request.StockPrices.Count == 0)
            return false;

        await _stockDbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE StockPrices");

        var stockPrices = new List<StockPrice>();

        foreach (var item in request.StockPrices)
        {
            var stockPrice = ReadPrice(item);
            if (stockPrice != null)
            {
                stockPrices.Add(stockPrice);
            }
        }

        var groupPrices = stockPrices.GroupBy(x => x.Symbol).Select(x => new { x.Key, Prices = x.OrderBy(y => y.Date).ToList() });
        foreach (var group in groupPrices)
        {
            for (int i = 1; i < group.Prices.Count; i++)
            {
                group.Prices[i].ChangeInPercent = (group.Prices[i].ClosePrice - group.Prices[i - 1].ClosePrice) / group.Prices[i - 1].ClosePrice * 100;
            }
        }

        if (stockPrices.Any())
        {
            await _stockDbContext.BulkInsertAsync(stockPrices);
        }

        return true;
    }

    private StockPrice? ReadPrice(string[] prices)
    {
        if (prices == null || prices.Length < 7)
            return null;

        if (!DateOnly.TryParseExact(prices[1], "yyyyMMdd", out DateOnly date)
            || !decimal.TryParse(prices[2], out decimal openPrice)
            || !decimal.TryParse(prices[3], out decimal highPrice)
            || !decimal.TryParse(prices[4], out decimal lowPrice)
            || !decimal.TryParse(prices[5], out decimal closePrice)
            || !long.TryParse(prices[6], out long volume)
            )
        {
            return null;
        }


        return new StockPrice()
        {
            Symbol = prices[0],
            Date = date,
            OpenPrice = openPrice,
            HighPrice = highPrice,
            LowPrice = lowPrice,
            ClosePrice = closePrice,
            Volume = volume
        };
    }
}
