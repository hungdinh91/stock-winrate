using Microsoft.AspNetCore.Mvc;
using StockAnalyzer.Application.Commands;
using StockAnalyzer.Domain.Dtos;
using StockAnalyzer.Domain.Entities;
using StockAnalyzer.Shared.CQRS;
using StockAnalyzer.Shared.Utils;

namespace StockAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly ILogger<StocksController> _logger;
        private readonly Mediator _mediator;

        public StocksController(ILogger<StocksController> logger, Mediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var startTime = DateTime.Now;
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded or file is empty.");
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var stockPrices = CsvReader.ReadCsv(filePath);
            await _mediator.Send(new InsertStockPricesCommand(stockPrices));

            return Ok((DateTime.Now - startTime).Seconds);
        }

        [HttpPost("[action]/{symbol}")]
        public async Task<IActionResult> CalculateRsi14(string symbol)
        {
            var startTime = DateTime.Now;

            await _mediator.Send(new CalculateRsi14Command(symbol));

            return Ok((DateTime.Now - startTime).Seconds);
        }

        [HttpPost("[action]/{symbol}")]
        public async Task<IActionResult> CalculateWinRate([FromRoute] string symbol, CalculateWinRateDto configDto)
        {
            var startTime = DateTime.Now;

            await _mediator.Send(new CalculateWinRateCommand(symbol, configDto));

            return Ok((DateTime.Now - startTime).Seconds);
        }

        [HttpPost("[action]/{symbol}")]
        public async Task<IActionResult> CalculateChanges([FromRoute] string symbol)
        {
            var startTime = DateTime.Now;

            await _mediator.Send(new CalculateChangesCommand(symbol));

            return Ok((DateTime.Now - startTime).Seconds);
        }
    }
}
