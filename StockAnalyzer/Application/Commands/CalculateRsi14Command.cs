﻿using Microsoft.EntityFrameworkCore;
using StockAnalyzer.Infrastructure;
using StockAnalyzer.Shared.CQRS;
using StockAnalyzer.Shared.Utils;

namespace StockAnalyzer.Application.Commands
{
    public class CalculateRsi14Command : IRequest<bool>
    {
    }

    public class CalculateRsi14CommandHandler : IRequestHandler<CalculateRsi14Command, bool>
    {
        private readonly StockDbContext _stockDbContext;

        public CalculateRsi14CommandHandler(StockDbContext stockDbContext)
        {
            _stockDbContext = stockDbContext;
        }

        public async Task<bool> Handle(CalculateRsi14Command request)
        {
            var prices = await _stockDbContext.StockPrices.ToListAsync();
            var groupPrices = prices.GroupBy(x => x.Symbol).Select(x => new { Symbol = x.Key, Prices = x.ToList() }).ToList();

            foreach (var group in groupPrices)
            {
                var orderedPrices = group.Prices.OrderBy(x => x.Date).ToList();
                for (var i = 0; i < orderedPrices.Count; i++)
                {
                    if (i + 1 < 14) continue;
                    var closePrices = orderedPrices.Take(i + 1).Select(x => (double)x.ClosePrice).ToArray();
                    var rsi =
                    orderedPrices[i].Rsi14 = StockIndexHelper.CalculateRSI(closePrices, 14);
                }
            }

            var affectedCount = await _stockDbContext.SaveChangesAsync();

            return await Task.FromResult(affectedCount > 0);
        }
    }
}
