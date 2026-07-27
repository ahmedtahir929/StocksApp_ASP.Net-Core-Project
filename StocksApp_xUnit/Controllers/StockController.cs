using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp_xUnit.Options;
using StocksApp_xUnit.ViewModels;

namespace StocksApp_xUnit.Controllers
{
    [Route("[controller]")]
    public class StockController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;

        public StockController(IOptions<TradingOptions> options, IFinnhubService finnhubService)
        {
            _tradingOptions = options.Value;
            _finnhubService = finnhubService;
        }

        [Route("[action]")]
        [Route("[action]/{stockSymbol?}")] //Allowing the route to accept the symbol
        public async Task<IActionResult> Explore(string? stockSymbol)
        {
            List<string> top25Stocks = _tradingOptions.Top25PopularStocks.Split(',').ToList();
            List<Dictionary<string, string>>? allStocks = await _finnhubService.GetStocks();

            if (allStocks == null)
            {
                //Return an empty list rather than null to prevent NullReferenceExceptions in the view
                return View(new List<Stock>());
            }

            List<Stock> stocks = allStocks
                .Where(stock => top25Stocks.Contains(stock["symbol"]))
                .Select(stock => new Stock
                {
                    StockSymbol = stock["symbol"],
                    StockName = stock["description"]
                })
                .OrderBy(temp => temp.StockName)
                .ToList();

            //Passing the selected symbol to the view so the ViewComponent knows what to load
            ViewBag.StockSymbol = stockSymbol;

            return View(stocks);
        }

        [Route("[action]/{stockSymbol}")]
        public IActionResult GetStockDetails(string stockSymbol)
        {
            //Executes the ViewComponent and returns its generated HTML
            return ViewComponent("SelectedStock", new { stockSymbol = stockSymbol });
        }
    }
}
