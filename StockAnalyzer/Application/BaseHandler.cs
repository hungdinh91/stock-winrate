using StockAnalyzer.Infrastructure;

namespace StockAnalyzer.Application
{
    public abstract class BaseHandler
    {
        protected StockDbContext _stockDbContext { get; set; }

        public BaseHandler(StockDbContext stockDbContext) { _stockDbContext = stockDbContext; }
    }
}
